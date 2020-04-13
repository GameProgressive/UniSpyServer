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
    public abstract class UpdateOptionHandlerBase : CommandHandlerBase
    {
        protected byte[] _clientRemoteIP;
        protected byte[] _gameServerDefaultHostPort;
        protected string _secretKey;
        protected ServerListRequest _request;
        protected List<byte> _dataList = new List<byte>();
        protected List<GameServer> _gameServers;
        public UpdateOptionHandlerBase(ServerListRequest request) : base()
        {
            _request = request;
        }

        protected override void CheckRequest(SBSession session, byte[] recv)
        {
            base.CheckRequest(session, recv);
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
            _clientRemoteIP = ((IPEndPoint)session.Socket.RemoteEndPoint).Address.GetAddressBytes();

            //TODO   //check what is the default port
            _gameServerDefaultHostPort = BitConverter.GetBytes((ushort)(6500 & 0xFFFF));
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
                bool skipThisServer = false;
                foreach (var key in _request.Keys)
                {
                    if (!server.ServerData.KeyValue.ContainsKey(key))
                    {
                        skipThisServer = true;
                        break;
                    }
                }

                if (skipThisServer)
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



        protected virtual void CheckPrivateIP(List<byte> header,  GameServer server)
        {
            // now we check if there are private ip
            if (server.ServerData.KeyValue.ContainsKey("localip0"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.ServerData.KeyValue["localip0"]).GetAddressBytes();
                header.AddRange(address);
            }
            else if (server.ServerData.KeyValue.ContainsKey("localip1"))
            {
                header[0] ^= (byte)GameServerFlags.PrivateIPFlag;
                byte[] address = IPAddress.Parse(server.ServerData.KeyValue["localip1"]).GetAddressBytes();
                header.AddRange(address);
            }
        }
        protected virtual void CheckNonStandardPort(List<byte> header,  GameServer server)
        {
            //we check host port is standard port or not
            if (server.ServerData.KeyValue.ContainsKey("hostport"))
            {
                if (server.ServerData.KeyValue["hostport"] != "6500")
                {
                    header[0] ^= (byte)GameServerFlags.NonStandardPort;
                    //we do not need to reverse port bytes
                    byte[] port = BitConverter.GetBytes(ushort.Parse(server.ServerData.KeyValue["hostport"]));

                    header.AddRange(port);
                }
            }
        }
        protected virtual void CheckPrivatePort(List<byte> header,  GameServer server)
        {
            // we check private port here
            if (server.ServerData.KeyValue.ContainsKey("localport"))
            {
                header[0] ^= (byte)GameServerFlags.NonStandardPrivatePortFlag;
                byte[] localPort =
                 BitConverter.GetBytes(ushort.Parse(server.ServerData.KeyValue["localport"]));

                header.AddRange(localPort);
            }
        }
        protected void CheckICMPSupport(List<byte> header,  GameServer server)
        {

        }

        /// <summary>
        /// Add ip and port
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        protected override void ConstructResponse(SBSession session, byte[] recv)
        {
            base.ConstructResponse(session, recv);
            _dataList.AddRange(_clientRemoteIP);
            _dataList.AddRange(_gameServerDefaultHostPort);
        }

        protected override void Response(SBSession session, byte[] recv)
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
            session.EncState = enc.State;

            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }

            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Send] { StringExtensions.ReplaceUnreadableCharToHex(_dataList.ToArray(), 0, _dataList.Count)}");
            session.BaseSendAsync(_sendingBuffer);
        }
    }
}
