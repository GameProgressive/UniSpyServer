using Newtonsoft.Json;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.NatNeg;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using StackExchange.Redis;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// we need forward this to game server
    /// </summary>
    internal sealed class NatNegMsgHandler : SBCmdHandlerBase
    {
        private new NatNegMsgRequest _request => (NatNegMsgRequest)base._request;
        private AdHocRequest _adHocRequest;
        private NatNegCookie _natNegCookie;
        private GameServerInfo _gameServer;
        public NatNegMsgHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            if (_session.ServerMessageList.Count == 0)
            {
                _result.ErrorCode = SBErrorCode.Parse;
                return;
            }
            _adHocRequest = _session.ServerMessageList.First();
            _session.ServerMessageList.Remove(_adHocRequest);

            var result = GameServerInfo.RedisOperator.GetMatchedKeyValues(_adHocRequest.TargetServerIP)
                .Values.Where(s => s.ServerData.KeyValue.ContainsKey("hostport"))
                .Where(s => s.ServerData.KeyValue["hostport"] == _adHocRequest.TargetServerHostPort);

            if (result.Count() != 1)
            {
                _result.ErrorCode = SBErrorCode.NoServersFound;
                return;
            }
            _gameServer = result.FirstOrDefault();
        }

        protected override void DataOperation()
        {
            //TODO check the if the remote endpoint is correct
            _natNegCookie.GameServerRemoteEndPoint = _gameServer.RemoteQueryReportIPEndPoint;
            _natNegCookie.GameServerRemoteIP = _adHocRequest.TargetServerIP;
            _natNegCookie.GameServerRemotePort = _gameServer.RemoteQueryReportPort;
            _natNegCookie.NatNegMessage = _request.NatNegMessage;
        }

        protected override void Response()
        {
            ISubscriber sub = UniSpyServerFactoryBase.Redis.GetSubscriber();
            string jsonStr = JsonConvert.SerializeObject(_natNegCookie);
            sub.Publish("NatNegCookieChannel", jsonStr);
        }
    }
}
