using QueryReport.Entity.Structure.NATNeg;
using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using ServerBrowser.Application;
using ServerBrowser.Entity.Exception;
using ServerBrowser.Entity.Structure.Request;
using System.Linq;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// we need forward this to game server
    /// </summary>
    internal sealed class NatNegMsgHandler : CmdHandlerBase
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
                throw new SBException("There are no server messages in _session.ServerMessageList.");
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
                throw new SBException("No server found in database.");
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
            SBServerFactory.Server.RedisChannelSubscriber.PublishMessage(_natNegCookie);
        }
    }
}
