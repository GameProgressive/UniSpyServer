using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class LoggedInCmdHandlerBase : CmdHandlerBase
    {
        public LoggedInCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
