using UniSpyServer.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("logout")]
    public sealed class LogoutHandler : CmdHandlerBase
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
