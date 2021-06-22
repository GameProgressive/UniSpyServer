using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    [Command("logout")]
    internal sealed class LogoutHandler : PCMCmdHandlerBase
    {
        public LogoutHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            _session.Disconnect();
        }
    }
}
