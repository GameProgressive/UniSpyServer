using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
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