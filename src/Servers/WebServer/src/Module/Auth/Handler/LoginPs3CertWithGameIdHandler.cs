
using UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{
    
    public sealed class LoginPs3CertWithGameIdHandler : LoginPs3CertHandler
    {
        private new LoginPs3CertWithGameIdRequest _request => (LoginPs3CertWithGameIdRequest)base._request;
        public LoginPs3CertWithGameIdHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}