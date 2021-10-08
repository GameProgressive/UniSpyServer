using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    internal abstract class LoggedInCmdHandlerBase : CmdHandlerBase
    {
        public LoggedInCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
