using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Handler
{
    [HandlerContract("LoginPs3CertWithGameId")]
    public sealed class LoginPs3CertWithGameIdHandler : LoginPs3CertHandler
    {
        private new LoginPs3CertWithGameIdRequest _request => (LoginPs3CertWithGameIdRequest)base._request;
        public LoginPs3CertWithGameIdHandler(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}