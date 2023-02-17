using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.General
{

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
