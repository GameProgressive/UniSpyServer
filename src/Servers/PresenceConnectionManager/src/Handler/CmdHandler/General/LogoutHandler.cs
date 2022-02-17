using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("logout")]
    public sealed class LogoutHandler : CmdHandlerBase
    {
        public LogoutHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _client.Session.Disconnect();
        }
    }
}
