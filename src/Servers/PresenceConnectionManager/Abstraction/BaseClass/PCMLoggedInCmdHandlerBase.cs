using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    internal abstract class PCMLoggedInCmdHandlerBase:PCMCmdHandlerBase
    {
        public PCMLoggedInCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
