using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Response
{
    public class LoginPs3CertWithGameIdResponse : LoginResponseBase
    {
        protected new LoginPs3CertWithGameIdRequest _request => (LoginPs3CertWithGameIdRequest)base._request;
        protected new LoginPs3CertResult _result => (LoginPs3CertResult)base._result;
        public LoginPs3CertWithGameIdResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            _content.Add("LoginPs3CertWithGameIdResult");
            _content.Add("responseCode", _result.ResponseCode);
            _content.Add("authToken", _result.AuthToken);
            _content.Add("partnerChallenge", _result.PartnerChallenge);
            base.Build();
        }
    }
}