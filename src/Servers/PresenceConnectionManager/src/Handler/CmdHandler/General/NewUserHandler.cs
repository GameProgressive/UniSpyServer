using PresenceConnectionManager.Entity.Contract;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal sealed class NewUserHandler : PresenceSearchPlayer.Handler.CmdHandler.NewUserHandler
    {
        public NewUserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NewUserResult();
        }
        protected override void ResponseConstruct()
        {
            _response = new NewUserResponse(_request, _result);
        }
    }
}
