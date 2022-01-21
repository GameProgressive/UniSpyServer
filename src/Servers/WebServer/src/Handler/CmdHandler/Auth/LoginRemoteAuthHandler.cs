using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler.Auth
{
    [HandlerContract("LoginRemoteAuth")]
    public class LoginRemoteAuthHandler : CmdHandlerBase
    {
        protected new LoginRemoteAuthRequest _request => (LoginRemoteAuthRequest)base._request;
        protected new LoginResult _result => (LoginResult)base._result;
        public LoginRemoteAuthHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}