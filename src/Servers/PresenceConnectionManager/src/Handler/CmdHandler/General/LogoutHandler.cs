using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
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
