using System;
using System.Collections.Generic;
using System.Text;
using QueryReport.Entity.Structure;
using Serilog.Events;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Packet.Response;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Result;
using ServerBrowser.Network;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class UpdateOptionHandlerBase : SBCmdHandlerBase
    {
        protected byte[] _clientRemoteIP { get; set; }
        protected byte[] _gameServerDefaultHostPort { get; set; }
        protected string _secretKey;
        protected new ServerListRequest _request => (ServerListRequest)base._request;
        protected new ServerListResult _result
        {
            get => (ServerListResult)base._result;
            set => base._result = value;
        }
        protected List<byte> _dataList { get; set; }
        protected List<GameServerInfo> _gameServers { get; set; }
        public UpdateOptionHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _dataList = new List<byte>();
            _result = new ServerListResult();
        }

        protected override void RequestCheck()
        {
            //we first check and get secrete key from database
            if (!DataOperationExtensions
                .GetSecretKey(_request.GameName, out _secretKey))
            {
                _result.ErrorCode = SBErrorCode.UnSupportedGame;
                return;
            }
            //this is client public ip and default query port
            _clientRemoteIP = _session.RemoteIPEndPoint.Address.GetAddressBytes();
            _result.ClientRemoteIP = _session.RemoteIPEndPoint.Address.GetAddressBytes();
            //TODO   //default port should be hton format
            _gameServerDefaultHostPort = ServerListRequest.HtonQueryReportDefaultPort;
        }

        protected override void ResponseConstruct()
        {
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

        protected bool IsSkipThisServer(GameServerInfo server)
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

        public static List<byte> GenerateServerInfoHeader(GameServerFlags flag, GameServerInfo server)
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


    }
}
