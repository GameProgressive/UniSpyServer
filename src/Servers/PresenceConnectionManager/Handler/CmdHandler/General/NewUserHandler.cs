using PresenceConnectionManager.Entity.Structure.Response;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal sealed class NewUserHandler : PresenceSearchPlayer.Handler.CmdHandler.NewUserHandler
    {
        public NewUserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void ResponseConstruct()
        {
            _response = new NewUserResponse(_request, _result);
        }
    }
}
