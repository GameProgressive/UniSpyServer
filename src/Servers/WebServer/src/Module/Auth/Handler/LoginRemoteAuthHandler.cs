using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Handler
{
    [HandlerContract("LoginRemoteAuth")]
    public class LoginRemoteAuthHandler : CmdHandlerBase
    {
        protected new LoginRemoteAuthRequest _request => (LoginRemoteAuthRequest)base._request;
        protected new LoginResult _result => (LoginResult)base._result;
        public LoginRemoteAuthHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}