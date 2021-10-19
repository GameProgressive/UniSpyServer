using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    /// <summary>
    /// Update user information (email)
    /// </summary>
    [HandlerContract("updateui")]
    public sealed class UpdateUIHandler : Abstraction.BaseClass.CmdHandlerBase
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
