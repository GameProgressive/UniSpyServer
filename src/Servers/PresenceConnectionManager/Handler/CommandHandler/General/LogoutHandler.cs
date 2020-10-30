using GameSpyLib.Abstraction.Interface;
using System.Collections.Generic;

namespace PresenceConnectionManager.Abstraction.BaseClass.General
{
    public class LogoutHandler : PCMCommandHandlerBase
    {
        public LogoutHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
        }

        protected override void DataOperation()
        {
            _session.Disconnect();
            PCMServer.LoggedInSession.Remove(_session.Id, out _);
        }
    }
}
