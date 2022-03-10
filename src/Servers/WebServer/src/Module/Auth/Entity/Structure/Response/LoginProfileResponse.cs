using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request;
using UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Response
{
    public class LoginProfileResponse : ResponseBase
    {
        protected new LoginProfileRequest _request => (LoginProfileRequest)base._request;
        protected new LoginResult _result => (LoginResult)base._result;
        public LoginProfileResponse(RequestBase request, ResultBase result) : base(request, result)
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

        private void AddCertificate()
        {
            var certElement = new XElement(SoapXElement.SakeNamespace + "certificate");
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "length", _result.Certificate.Length));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "version", _request.Version));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "partnercode", _request.PartnerCode));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "namespaceid", _request.NamespaceId));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "userid", _result.Certificate.UserId));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "profileid", _result.Certificate.ProfileId));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "expiretime", _result.Certificate.ExpireTime));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "profilenick", _result.Certificate.ProfileNick));

            certElement.Add(new XElement(SoapXElement.SakeNamespace + "uniquenick", _request.Uniquenick));

            certElement.Add(new XElement(SoapXElement.SakeNamespace + "cdkeyhash", _result.Certificate.CdKeyHash));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeymodulus", _result.Certificate.PeerKeyModulus));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeyexponent", _result.Certificate.PeerKeyExponent));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "serverdata", _result.Certificate.ServerData));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "signature", _result.Certificate.Signature));

            _soapElement.Add(certElement);
        }
    }
}