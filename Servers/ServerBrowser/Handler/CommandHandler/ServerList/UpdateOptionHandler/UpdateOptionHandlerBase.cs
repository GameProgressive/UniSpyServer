using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Encryption;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using QueryReport.Entity.Structure;
using Serilog.Events;
using ServerBrowser.Entity.Enumerator;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Request;
using ServerBrowser.Entity.Structure.Packet.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ServerBrowser.Handler.CommandHandler.ServerList.UpdateOptionHandler
{
    public abstract class UpdateOptionHandlerBase : SBCommandHandlerBase
    {
        protected byte[] _clientRemoteIP;
        protected byte[] _gameServerDefaultHostPort;
        protected string _secretKey;
        protected ServerListRequest _request;
        protected List<byte> _dataList;
        protected List<GameServer> _gameServers;
        public UpdateOptionHandlerBase(ServerListRequest request, ISession client, byte[] recv) : base(client, recv)
        {
            _request = request;
            _dataList = new List<byte>();
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            //save client challenge in _request
            if (_request == null)
            {
                _errorCode = SBErrorCode.Parse;
                return;
            }
            //we first check and get secrete key from database
            if (!DataOperationExtensions.GetSecretKey(_request.GameName, out _secretKey))
            {
                _errorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            //this is client public ip and default query port
            SBSession session = (SBSession)_session.GetInstance();
            _clientRemoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();

            //TODO   //default port should be hton format
            ushort defaultPort = 6500;
            byte[] defaultPortBytes = BitConverter.GetBytes(defaultPort);
            Array.Reverse(defaultPortBytes);
            ushort htonDefaultPort = BitConverter.ToUInt16(defaultPortBytes);
            _gameServerDefaultHostPort = BitConverter.GetBytes(htonDefaultPort);
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
            _dataList.AddRange(_clientRemoteIP);
            _dataList.AddRange(_gameServerDefaultHostPort);
        }

        protected override void Response()
        {
            GOAEncryption enc =
                new GOAEncryption(_secretKey, _request.Challenge, SBServer.ServerChallenge);

            _sendingBuffer = new ServerListResponse().
                CombineHeaderAndContext
                (
                    enc.Encrypt(_dataList.ToArray()),
                     SBServer.ServerChallenge
                );
            //refresh encryption state
            SBSession session = (SBSession)_session.GetInstance();
            session.EncState = enc.State;

            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }

            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Send] { StringExtensions.ReplaceUnreadableCharToHex(_dataList.ToArray(), 0, _dataList.Count)}");
            session.BaseSendAsync(_sendingBuffer);
        }

        protected virtual void GenerateServerKeys()
        {
            //we add the total number of the requested keys
            _dataList.Add((byte)_request.Keys.Length);
            //then we add the keys
            foreach (var key in _request.Keys)
            {
                _dataList.Add((byte)SBKeyType.String);
                _dataList.AddRange(Encoding.ASCII.GetBytes(key));
                _dataList.Add(0);
            }
        }

        protected virtual void GenerateUniqueValue()
        {
            //because we are using NTS string so we do not have any value here
            _dataList.Add(0);
        }

        protected virtual void GenerateServersInfo()
        {
            foreach (var server in _gameServers)
            {
                //we check the server
                //This is the way we can not crash by some
                //fake server
                if (IsSkipThisServer(server))
                {
                    continue;
                }

                List<byte> header = new List<byte>();
                GenerateServerInfoHeader(header, server);
                _dataList.AddRange(header);
                foreach (var key in _request.Keys)
                {
                    _dataList.Add(SBStringFlag.NTSStringFlag);
                    _dataList.AddRange(Encoding.ASCII.GetBytes(server.ServerData.KeyValue[key]));
                    _dataList.Add(SBStringFlag.StringSpliter);
                }
            }
        }

        protected bool IsSkipThisServer(GameServer server)
        {

            foreach (var key in _request.Keys)
            {
                if (!server.ServerData.KeyValue.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual void GenerateServerInfoHeader(List<byte> header, GameServer server)
        {
            //add has key flag
            header.Add((byte)GameServerFlags.HasKeysFlag);
            //we add server public ip here
            header.AddRange(ByteTools.GetIPBytes(server.RemoteIP));
            //we check host port is standard port or not
            CheckNonStandardPort(header, server);
            // now we check if there are private ip
            CheckPrivateIP(header, server);
            // we check private port here
            CheckPrivatePort(header, server);
            //we check icmp support here
            CheckICMPSupport(header, server);
        }

        protected virtual void CheckPrivateIP(List<byte> header, GameServer server)
        {
            string localIP = "";
            // now we check if there are private ip
            if (server.ServerData.KeyValue.ContainsKey("localip0"))
            {
                localIP = server.ServerData.KeyValue["localip0"];
            }
            else if (server.ServerData.KeyValue.ContainsKey("localip1"))
            {
                localIP = server.ServerData.KeyValue["localip1"];
            }
            if (localIP == "")
            {
                return;
            }
            header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
            byte[] address = IPAddress.Parse(localIP).GetAddressBytes();
            header.AddRange(address);

        }
        protected virtual void CheckNonStandardPort(List<byte> header, GameServer server)
        {
            //we check host port is standard port or not
            if (!server.ServerData.KeyValue.ContainsKey("hostport"))
            {
                return;
            }
            if (server.ServerData.KeyValue["hostport"] == "")
            {
                return;
            }

            if (server.ServerData.KeyValue["hostport"] != "6500")
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPort;
                //we do not need to reverse port bytes
                byte[] port = BitConverter.GetBytes(ushort.Parse(server.ServerData.KeyValue["hostport"]));

                header.AddRange(port);
            }
        }
        protected virtual void CheckPrivatePort(List<byte> header, GameServer server)
        {
            // we check private port here
            if (!server.ServerData.KeyValue.ContainsKey("privateport"))
            {
                return;
            }
            if (server.ServerData.KeyValue["privateport"] == "")
            {
                return;
            }

            header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
            byte[] localPort =
             BitConverter.GetBytes(ushort.Parse(server.ServerData.KeyValue["localport"]));

            header.AddRange(localPort);
        }
        protected void CheckICMPSupport(List<byte> header, GameServer server)
        {
            header[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            string localIP = server.RemoteIP;
            byte[] address = IPAddress.Parse(localIP).GetAddressBytes();
            header.AddRange(address);
        }
    }
}
