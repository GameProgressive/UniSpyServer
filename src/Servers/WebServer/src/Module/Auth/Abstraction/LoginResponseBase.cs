using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Abstraction
{

    public abstract class LoginResponseBase : ResponseBase
    {
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        protected new LoginRequestBase _request => (LoginRequestBase)base._request;
        protected LoginResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        protected void BuildContext()
        {
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "responseCode", _result.ResponseCode));

            var certElement = new XElement(SoapXElement.SakeNamespace + "certificate");
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "length", _result.Length));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "version", _request.Version));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "partnercode", _request.PartnerCode));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "namespaceid", _request.NamespaceId));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "userid", _result.UserId));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "profileid", _result.ProfileId));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "expiretime", ClientInfo.ExpireTime));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "profilenick", _result.ProfileNick));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "uniquenick", _result.UniqueNick));

            certElement.Add(new XElement(SoapXElement.SakeNamespace + "cdkeyhash", _result.CdKeyHash));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeymodulus", ClientInfo.PeerKeyModulus));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeyexponent", ClientInfo.PeerKeyExponent));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "serverdata", ClientInfo.ServerData));
            certElement.Add(new XElement(SoapXElement.SakeNamespace + "signature", ClientInfo.Signature));

            _soapElement.Add(certElement);
            _soapElement.Add(new XElement(SoapXElement.SakeNamespace + "peerkeyprivate", ClientInfo.PeerKeyPrivate));
        }
    }
}