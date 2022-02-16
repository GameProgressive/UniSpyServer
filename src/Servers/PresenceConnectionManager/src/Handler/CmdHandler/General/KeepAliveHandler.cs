using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("ka")]
    public sealed class KeepAliveHandler : CmdHandlerBase
    {
        public KeepAliveHandler(IClient client, IRequest request) : base(client, request)
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
