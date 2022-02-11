using System.Linq;
using UniSpyServer.Servers.QueryReport.Entity.Structure.NATNeg;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Application;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Handler.CmdHandler
{
    /// <summary>
    /// we need forward this to game server
    /// </summary>
    public sealed class NatNegMsgHandler : CmdHandlerBase
    {
        private new NatNegMsgRequest _request => (NatNegMsgRequest)base._request;
        private ServerInfoRequest _adHocRequest;
        private NatNegCookie _natNegCookie;
        private GameServerInfo _gameServer;
        public NatNegMsgHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();
            if (_session.ServerMessageStack.Count == 0)
            {
                throw new SBException("There are no server messages in _session.ServerMessageList.");
            }
            _adHocRequest = _session.ServerMessageStack.First();
            _session.ServerMessageStack.Remove(_adHocRequest);

            _gameServer = _gameServerRedisClient.Values.Where(x =>
                x.HostIPAddress == _adHocRequest.TargetIPEndPoint.Address &
                x.HostPort == (ushort)_adHocRequest.TargetIPEndPoint.Port)
                .FirstOrDefault();
            if (_gameServer == null)
            {
                throw new SBException("There is no matching game server regesterd.");
            }
        }

        protected override void DataOperation()
        {
            //TODO check the if the remote endpoint is correct
            _natNegCookie = new NatNegCookie
            {
                HeartBeatIPEndPoint = _gameServer.HeartBeatIPEndPoint,
                HostIPAddress = _adHocRequest.TargetIPEndPoint.Address,
                HostPort = (ushort)_gameServer.HostPort,
                NatNegMessage = _request.RawRequest,
                InstantKey = (uint)_gameServer.InstantKey,
                GameName = _gameServer.GameName
            };
        }

        protected override void Response()
        {
            ServerFactory.Server.RedisChannelSubscriber.PublishMessage(_natNegCookie);
        }
    }
}
