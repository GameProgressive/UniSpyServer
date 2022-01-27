using UniSpyServer.Servers.QueryReport.Entity.Structure.NATNeg;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using UniSpyServer.Servers.ServerBrowser.Application;
using UniSpyServer.Servers.ServerBrowser.Entity.Exception;
using UniSpyServer.Servers.ServerBrowser.Entity.Structure.Request;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;

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
        public NatNegMsgHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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

            var gameServer = _gameServerRedisClient.Values.Where(x => x.RemoteIPEndPoint == _adHocRequest.TargetIPEndPoint).FirstOrDefault();
            if (gameServer == null)
            {
                throw new SBException("There is no matching game server regesterd.");
            }
            if (!gameServer.ServerData.Keys.Contains("hostport"))
            {
                throw new SBException("There is no hostport information in game server.");
            }
            if (gameServer.ServerData["hostport"] == _adHocRequest.TargetServerHostPort)
            {
                _gameServer = gameServer;
            }
            else
            {
                throw new SBException("The game server host port do not match to request host port.");
            }
        }

        protected override void DataOperation()
        {
            //TODO check the if the remote endpoint is correct
            _natNegCookie = new NatNegCookie
            {
                GameServerRemoteEndPoint = _gameServer.RemoteIPEndPoint,
                GameServerRemoteIP = _adHocRequest.TargetServerIP,
                GameServerRemotePort = _gameServer.RemoteIPEndPoint.Port.ToString(),
                NatNegMessage = _request.RawRequest
            };
        }

        protected override void Response()
        {
            ServerFactory.Server.RedisChannelSubscriber.PublishMessage(_natNegCookie);
        }
    }
}
