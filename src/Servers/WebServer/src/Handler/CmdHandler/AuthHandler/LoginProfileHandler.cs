using UniSpyLib.Abstraction.Interface;
using WebServer.Abstraction;

namespace WebServer.Handler.CmdHandler.AuthHandler
{
    internal class LoginProfileHandler : CmdHandlerBase
    {
        public LoginProfileHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}