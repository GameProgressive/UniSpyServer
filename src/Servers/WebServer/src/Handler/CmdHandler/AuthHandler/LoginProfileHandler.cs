using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler.AuthHandler
{
    public class LoginProfileHandler : CmdHandlerBase
    {
        public LoginProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}