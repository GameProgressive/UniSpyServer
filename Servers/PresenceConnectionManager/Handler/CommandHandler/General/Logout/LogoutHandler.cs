using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.General.Logout
{
    public class LogoutHandler : CommandHandlerBase
    {
        protected LogoutHandler() : base()
        {
        }

        protected override void DataOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            GPCMServer.LoggedInSession.Remove(session.Id, out _);
        }
    }
}
