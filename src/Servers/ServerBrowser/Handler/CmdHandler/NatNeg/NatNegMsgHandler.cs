using Newtonsoft.Json;
using QueryReport.Entity.Structure.NatNeg;
using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Entity.Enumerate;
using ServerBrowser.Entity.Structure.Request;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Entity.Structure;

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

            var searchKey = new GameServerInfoRedisKey()
            {
                RemoteIPEndPoint = _adHocRequest.TargetIPEndPoint
            };

            var matchedKeys = GameServerInfoRedisOperator.GetMatchedKeyValues(searchKey)
                .Values.Where(s => s.ServerData.KeyValue.ContainsKey("hostport"))
                .Where(s => s.ServerData.KeyValue["hostport"] == _adHocRequest.TargetServerHostPort);

            if (matchedKeys.Count() != 1)
            {
                _result.ErrorCode = SBErrorCode.NoServersFound;
                return;
            }
            _gameServer = matchedKeys.First();
        }

        protected override void DataOperation()
        {            
            //TODO check the if the remote endpoint is correct
            _natNegCookie = new NatNegCookie
            {
                GameServerRemoteEndPoint = _gameServer.RemoteQueryReportIPEndPoint,
                GameServerRemoteIP = _adHocRequest.TargetServerIP,
                GameServerRemotePort = _gameServer.RemoteQueryReportPort,
                NatNegMessage = _request.NatNegMessage
            };
        }

        protected override void Response()
        {
            var subscriber = UniSpyServerFactoryBase.Redis.GetSubscriber();
            string jsonStr = JsonConvert.SerializeObject(_natNegCookie);
            subscriber.Publish(UniSpyRedisChannelName.NatNegCookieChannel, jsonStr);
        }
    }
}
