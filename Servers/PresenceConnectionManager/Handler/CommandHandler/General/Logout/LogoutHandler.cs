using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.CommandHandler.General.Logout
{
    public class LogoutHandler : GPCMHandlerBase
    {
        protected LogoutHandler(GPCMSession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void DataBaseOperation(GPCMSession session, Dictionary<string, string> recv)
        {
            GPCMServer.LoggedInSession.Remove(session.Id, out _);
        }
    }
}
