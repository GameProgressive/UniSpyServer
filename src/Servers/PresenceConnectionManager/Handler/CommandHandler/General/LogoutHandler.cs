using UniSpyLib.Abstraction.Interface;
using System.Collections.Generic;
using PresenceConnectionManager.Network;
using PresenceConnectionManager.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandHandler
{
    public class LogoutHandler : PCMCommandHandlerBase
    {
        public LogoutHandler(ISession session,IRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.Disconnect();
            PCMServer.LoggedInSession.Remove(_session.Id, out _);
        }
    }
}
