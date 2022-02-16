using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Handler.CmdHandler.Auth
{
    [HandlerContract("LoginPs3Cert")]
    public class LoginPs3CertHandler : CmdHandlerBase
    {
        protected new LoginPs3CertRequest _request => (LoginPs3CertRequest)base._request;
        protected new LoginPs3CertResult _result => (LoginPs3CertResult)base._result;
        public LoginPs3CertHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}