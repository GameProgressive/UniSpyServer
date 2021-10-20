using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.PresenceConnectionManager.Handler.CmdHandler
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
