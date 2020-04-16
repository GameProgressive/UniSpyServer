using System.Collections.Generic;
using GameSpyLib.Common.Entity.Interface;

namespace PresenceConnectionManager.Handler.CommandHandler.General.Logout
{
    public class LogoutHandler :  PCMCommandHandlerBase
    {
        public LogoutHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
        }

        protected override void DataOperation()
        {
            GPCMServer.LoggedInSession.Remove(_session.Id, out _);
        }
    }
}
