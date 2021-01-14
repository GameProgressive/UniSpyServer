using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Network;
using System.Collections.Generic;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class LogoutHandler : PCMCmdHandlerBase
    {
        public LogoutHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.Disconnect();
            PCMServer.LoggedInSession.Remove(_session.Id, out _);
        }
    }
}
