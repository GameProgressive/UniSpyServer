using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("ka")]
    public sealed class KeepAliveHandler : CmdHandlerBase
    {
        public KeepAliveHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            //we need to keep the player cache online
            //so that other players can find the player
        }

        protected override void ResponseConstruct()
        {
            _response = new KeepAliveResponse(_request, _result);
        }
    }
}
