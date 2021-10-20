using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.WebServer.Abstraction;

namespace UniSpyServer.WebServer.Handler.CmdHandler.AuthHandler
{
    public class LoginProfileHandler : CmdHandlerBase
    {
        public LoginProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}