
using UniSpy.Server.WebServer.Module.Auth.Contract.Request;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.WebServer.Application;

namespace UniSpy.Server.WebServer.Module.Auth.Handler
{
    
    public sealed class LoginPs3CertWithGameIdHandler : LoginPs3CertHandler
    {
        private new LoginPs3CertWithGameIdRequest _request => (LoginPs3CertWithGameIdRequest)base._request;
        public LoginPs3CertWithGameIdHandler(Client client, LoginPs3CertWithGameIdRequest request) : base(client, request)
        {
        }
    }
}