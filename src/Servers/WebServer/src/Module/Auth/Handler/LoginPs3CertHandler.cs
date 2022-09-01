using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Handler
{
    
    public class LoginPs3CertHandler : CmdHandlerBase
    {
        protected new LoginPs3CertRequest _request => (LoginPs3CertRequest)base._request;
        protected new LoginPs3CertResult _result => (LoginPs3CertResult)base._result;
        public LoginPs3CertHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}