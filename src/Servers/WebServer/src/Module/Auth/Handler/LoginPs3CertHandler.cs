using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
using UniSpy.Server.WebServer.Module.Auth.Contract.Result;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{

    public class LoginPs3CertHandler : CmdHandlerBase
    {
        protected new LoginPs3CertRequest _request => (LoginPs3CertRequest)base._request;
        protected new LoginPs3CertResult _result => (LoginPs3CertResult)base._result;
        public LoginPs3CertHandler(Client client, LoginPs3CertRequest request) : base(client, request)
        {
        }
    }
}