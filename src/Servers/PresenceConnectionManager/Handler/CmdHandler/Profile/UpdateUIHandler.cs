using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    [Command("updateui")]
    internal sealed class UpdateUIHandler : PCMCmdHandlerBase
    {
        public UpdateUIHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            //todo find what data is belong to user info
            throw new System.NotImplementedException();
        }

        protected override void DataOperation()
        {
            throw new System.NotImplementedException();
        }
    }
}
