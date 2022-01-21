using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Response.Auth
{
    public class LoginProfileResponse : ResponseBase
    {
        protected new LoginProfileRequest _request => (LoginProfileRequest)base._request;
        protected new LoginResult _result => (LoginResult)base._result;
        public LoginProfileResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            _soapElement.Add(new XElement(SoapXElement.SoapNamespace + "LoginProfileWithGameIdResult"));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "responseCode", _result.ResponseCode));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "certificate", _result.Certificate));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeyprivate", _result.PeerKeyPrivate));
        }
    }
}