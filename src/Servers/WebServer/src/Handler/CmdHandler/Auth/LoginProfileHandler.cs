using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler.Auth
{
    [HandlerContract("LoginProfile")]
    public class LoginProfileHandler : CmdHandlerBase
    {
        public LoginProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}