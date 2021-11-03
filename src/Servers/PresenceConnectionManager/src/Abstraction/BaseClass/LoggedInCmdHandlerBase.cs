using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class LoggedInCmdHandlerBase : CmdHandlerBase
    {
        public LoggedInCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
