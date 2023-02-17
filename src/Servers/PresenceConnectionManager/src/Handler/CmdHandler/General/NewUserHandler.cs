using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;


namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.General
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
