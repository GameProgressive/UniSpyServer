using QueryReport.Entity.Structure;
using Serilog.Events;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Misc;
using ServerBrowser.Entity.Structure.Request;
using ServerBrowser.Entity.Structure.Result;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Encryption;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// Get full rules for a server (for example, to get
    /// player information from a server that only has basic information so far)
    /// </summary>
    internal class ServerInfoHandler : SBCmdHandlerBase
    {
        protected new AdHocRequest _request => (AdHocRequest)base._request;
        protected new ServerInfoResult _result
        {
            get => (ServerInfoResult)base._result;
            set => base._result = value;
        }
        protected List<byte> _dataList;

        public ServerInfoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _dataList = new List<byte>();
        }

        protected override void DataOperation()
        {
            var result = GameServerInfo.RedisOperator.GetMatchedKeyValues(_request.TargetServerIP)
                .Values.Where(s => s.ServerData.KeyValue.ContainsKey("hostport"))
                .Where(s => s.ServerData.KeyValue["hostport"] == _request.TargetServerHostPort);
            //TODO if there are no server found, we still send response back to client
            if (result.Count() != 1)
            {
                _result.ErrorCode = SBErrorCode.NoServersFound;
                return;
            }
            _result.GameServerInfo = result.FirstOrDefault();
        }

        protected override void ResponseConstruct()
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
            // we need to check whether enc state will be given new value;
            // _session.EncState = enc.State;
        }

        protected virtual List<byte> GenerateServerInfo()
        {
            List<byte> data = new List<byte>();
            data.AddRange(
                UpdateOptionHandlerBase.GenerateServerInfoHeader(
                    GameServerFlags.HasFullRulesFlag, _result.GameServerInfo));

            foreach (var kv in _result.GameServerInfo.ServerData.KeyValue)
            {
                data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                data.Add(SBStringFlag.StringSpliter);
                data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                data.Add(SBStringFlag.StringSpliter);
            }

            foreach (var player in _result.GameServerInfo.PlayerData.KeyValueList)
            {
                foreach (var kv in player)
                {
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Key));
                    data.Add(SBStringFlag.StringSpliter);
                    data.AddRange(Encoding.ASCII.GetBytes(kv.Value));
                    data.Add(SBStringFlag.StringSpliter);
                }
            }

            foreach (var team in _result.GameServerInfo.TeamData.KeyValueList)
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
