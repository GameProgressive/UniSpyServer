using UniSpyLib.Abstraction.Interface;
using WebServer.Abstraction;

namespace WebServer.Handler.CmdHandler.AuthHandler
{
    public class LoginProfileHandler : CmdHandlerBase
    {
        public LoginProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}