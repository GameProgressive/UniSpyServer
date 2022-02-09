using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth;
using UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Response.Auth
{
    public class LoginUniqueNickResponse : ResponseBase
    {
        protected new LoginRemoteAuthRequest _request => (LoginRemoteAuthRequest)base._request;
        protected new LoginResult _result => (LoginResult)base._result;
        public LoginUniqueNickResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            base.Build();
            _soapElement.Add(new XElement(SoapXElement.SoapNamespace + "LoginUniqueNickWithGameIdResult"));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "responseCode", _result.ResponseCode));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "certificate", _result.Certificate));
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeyprivate", _result.PeerKeyPrivate));
        }
    }
}