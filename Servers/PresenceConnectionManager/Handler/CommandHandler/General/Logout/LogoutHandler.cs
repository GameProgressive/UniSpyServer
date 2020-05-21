using GameSpyLib.Common.Entity.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.CommandHandler.General.Logout
{
    public class LogoutHandler : PCMCommandHandlerBase
    {
        public LogoutHandler(ISession client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void DataOperation()
        {
            PCMServer.LoggedInSession.Remove(_session.Id, out _);
        }
    }
}
