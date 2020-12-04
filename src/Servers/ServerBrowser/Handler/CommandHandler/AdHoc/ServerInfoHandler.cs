using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryReport.Entity.Structure;
using Serilog.Events;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure;
using ServerBrowser.Entity.Structure.Request;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CommandHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    public class ServerInfoHandler : UpdateOptionHandlerBase
    {
        private new AdHocRequest _request;
        private GameServer _gameServer;

        public ServerInfoHandler(ISession session, IRequest request) : base(session, request)
        {
            _request = (AdHocRequest)request;
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            var result = GameServer.GetServers(_request.TargetServerIP)
                .Where(s => s.ServerData.KeyValue.ContainsKey("hostport"))
                .Where(s => s.ServerData.KeyValue["hostport"] == _request.TargetServerHostPort);

            if (result.Count() != 1)
            {
                _errorCode = SBErrorCode.NoServersFound;
                return;
            }
            _gameServer = result.FirstOrDefault();
        }

        protected override void ConstructResponse()
        {
            _dataList.Add((byte)SBServerResponseType.PushServerMessage);

            byte[] info = GenerateServerInfo().ToArray();

            // we add server info here
            _dataList.AddRange(info);
            LogWriter.ToLog("[Plain] " +
                StringExtensions.ReplaceUnreadableCharToHex(info));

            byte[] msgLength =
                ByteTools.GetBytes((short)(info.Length + 2), true);

            _dataList.InsertRange(0, msgLength);

            GOAEncryption enc = new GOAEncryption(_session.EncState);
            _sendingBuffer = enc.Encrypt(_dataList.ToArray());
            _session.EncState = enc.State;
        }

        protected virtual List<byte> GenerateServerInfo()
        {
            List<byte> data = new List<byte>();
            data.AddRange(GenerateServerInfoHeader(_gameServer));

            foreach (var kv in _gameServer.ServerData.KeyValue)
            {
                data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                data.Add(SBStringFlag.StringSpliter);
                data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                data.Add(SBStringFlag.StringSpliter);
            }

            foreach (var player in _gameServer.PlayerData.KeyValueList)
            {
                foreach (var kv in player)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }

            foreach (var team in _gameServer.TeamData.KeyValueList)
            {
                foreach (var kv in team)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }

            return data;
        }

        protected virtual List<byte> GenerateServerInfoHeader(GameServer server)
        {
            // you will only have HasKeysFlag or HasFullRule you can not have both
            List<byte> header = new List<byte>();

            //add has key flag
            header.Add((byte)GameServerFlags.HasFullRulesFlag);

            //we add server public ip here
            header.AddRange(ByteTools.GetIPBytes(server.RemoteQueryReportIP));

            //we check host port is standard port or not
            CheckNonStandardPort(header, server);

            // now we check if there are private ip
            CheckPrivateIP(header, server);

            // we check private port here
            CheckPrivatePort(header, server);

            //TODO we have to check icmp_ip_flag
            CheckICMPSupport(header, server);

            return header;
        }

        protected override void Response()
        {
            if (_sendingBuffer == null || _sendingBuffer.Length < 4)
            {
                return;
            }
            LogWriter.ToLog(LogEventLevel.Debug,
              $"[Send] { StringExtensions.ReplaceUnreadableCharToHex(_dataList.ToArray(), 0, _dataList.Count)}");
            _session.BaseSendAsync(_sendingBuffer);
        }
    }
}
