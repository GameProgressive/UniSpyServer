using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;
using QueryReport.Entity.Structure;
using Serilog.Events;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Packet.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ServerBrowser.Network;
using ServerBrowser.Entity.Structure.Request;

namespace ServerBrowser.Abstraction.BaseClass
{
    public abstract class UpdateOptionHandlerBase : SBCmdHandlerBase
    {
        protected byte[] _clientRemoteIP;
        protected byte[] _gameServerDefaultHostPort;
        protected string _secretKey;
        protected new ServerListRequest _request
        {
            get { return (ServerListRequest)base._request; }
            set { base._request = value; }
        }
        protected List<byte> _dataList;
        protected List<GameServer> _gameServers;

        public UpdateOptionHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
            if (!DataOperationExtensions
                .GetSecretKey(_request.GameName, out _secretKey))
            {
                _errorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            //this is client public ip and default query port

            _clientRemoteIP = ((IPEndPoint)_session.Socket.RemoteEndPoint).Address.GetAddressBytes();

            //TODO   //default port should be hton format
            byte[] defaultPortBytes = BitConverter.GetBytes(
                ServerListRequest.QueryReportDefaultPort);

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
            _session.EncState = enc.State;

            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }

            LogWriter.ToLog(LogEventLevel.Debug,
                $"[Send] { StringExtensions.ReplaceUnreadableCharToHex(_dataList.ToArray(), 0, _dataList.Count)}");
            _session.BaseSendAsync(_sendingBuffer);
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
            //TODO some game use this, so we have to complete two methods
            //TODO
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

                List<byte> header = GenerateServerInfoHeader(GameServerFlags.HasKeysFlag, server);
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
                //do not skip empty value
                if (!server.ServerData.KeyValue.ContainsKey(key))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<byte> GenerateServerInfoHeader(GameServerFlags flag, GameServer server)
        {
            List<byte> header = new List<byte>();
            //add has key flag
            header.Add((byte)flag);
            //we add server public ip here
            header.AddRange(ByteTools.GetIPBytes(server.RemoteQueryReportIP));
            //we check host port is standard port or not
            CheckNonStandardPort(header, server);
            // now we check if there are private ip
            CheckPrivateIP(header, server);
            // we check private port here
            CheckPrivatePort(header, server);
            //we check icmp support here
            CheckICMPSupport(header, server);

            return header;
        }

        public static void CheckPrivateIP(List<byte> header, GameServer server)
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
            byte[] address = HtonsExtensions.IPStringToBytes(localIP);
            header.AddRange(address);
        }

        public static void CheckNonStandardPort(List<byte> header, GameServer server)
        {
            ///only dedicated server have different query report port and host port
            ///the query report port and host port are the same on peer server
            ///so we do not need to check this for peer server
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
                //we do not need htons here
                byte[] port =
                     HtonsExtensions.PortToIntBytes(
                         server.ServerData.KeyValue["hostport"]);
                byte[] htonPort =
                    HtonsExtensions.UshortPortToHtonBytes(
                        server.ServerData.KeyValue["hostport"]);
                header.AddRange(htonPort);
            }
        }

        public static void CheckPrivatePort(List<byte> header, GameServer server)
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

            byte[] port =
                HtonsExtensions.PortToIntBytes(
                    server.ServerData.KeyValue["privateport"]);

            header.AddRange(port);
        }

        public static void CheckICMPSupport(List<byte> header, GameServer server)
        {
            header[0] ^= (byte)GameServerFlags.ICMPIPFlag;
            byte[] address = HtonsExtensions.IPStringToBytes(server.RemoteQueryReportIP);
            header.AddRange(address);
        }
    }
}
