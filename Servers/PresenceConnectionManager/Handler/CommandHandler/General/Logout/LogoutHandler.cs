using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.CommandHandler.General.Logout
{
    public class LogoutHandler : GPCMHandlerBase
    {
        protected override void DataBaseOperation(GPCMSession session)
        {
            GPCMServer.LoggedInSession.Remove(session.Id, out _);
        }
    }
}
