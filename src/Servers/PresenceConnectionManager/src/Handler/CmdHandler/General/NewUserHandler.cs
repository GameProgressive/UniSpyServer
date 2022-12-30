using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;


namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler.General
{
    public sealed class NewUserHandler : PresenceSearchPlayer.Handler.CmdHandler.NewUserHandler
    {
        public NewUserHandler(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void ResponseConstruct()
        {
            _response = new NewUserResponse(_request, _result);
        }
    }
}
