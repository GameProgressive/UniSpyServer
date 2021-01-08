using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    public class NewUserHandler : PresenceSearchPlayer.Handler.CmdHandler.NewUserHandler
    {
        public NewUserHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
