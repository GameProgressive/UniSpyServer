using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.PresenceConnectionManager.Handler.CmdHandler
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
