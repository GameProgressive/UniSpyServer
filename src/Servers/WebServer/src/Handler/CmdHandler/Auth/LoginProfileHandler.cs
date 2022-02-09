using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler.Auth
{
    [HandlerContract("LoginProfile")]
    public class LoginProfileHandler : CmdHandlerBase
    {
        protected new LoginProfileRequest _request => (LoginProfileRequest)base._request;
        protected new LoginResult _result => (LoginResult)base._result;
        public LoginProfileHandler(ISession session, IRequest request) : base(session, request)
        {
        }
    }
}