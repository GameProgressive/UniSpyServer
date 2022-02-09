using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Response.Auth
{
    public class LoginPs3CertResponse : ResponseBase
    {
        protected new LoginPs3CertRequest _request => (LoginPs3CertRequest)base._request;
        protected new LoginPs3CertResult _result => (LoginPs3CertResult)base._result;
        public LoginPs3CertResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            _soapElement.Add(new XElement(SoapXElement.SoapNamespace + "LoginPs3CertWithGameIdResult"));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "responseCode", _result.ResponseCode));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "authToken", _result.AuthToken));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "partnerChallenge", _result.PartnerChallenge));
        }
    }
}