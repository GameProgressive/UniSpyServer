using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Abstraction
{

    public abstract class LoginResponseBase : ResponseBase
    {
        protected new LoginResultBase _result => (LoginResultBase)base._result;
        protected new LoginRequestBase _request => (LoginRequestBase)base._request;
        protected LoginResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
            _soapEnvelop = new SoapXElement(SoapXElement.AuthSoapHeader);
            _soapBody = new XElement(SoapXElement.SoapNamespace + "Body");
        }

        protected void BuildContext()
        {
            // find the node with command name
            var context = _soapBody.Elements().First();
            context.Add(new XElement(SoapXElement.AuthNamespace + "responseCode", _result.ResponseCode));
            var certElement = new XElement(SoapXElement.AuthNamespace + "certificate");
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "length", _result.Length));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "version", _request.Version));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "partnercode", _request.PartnerCode));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "namespaceid", _request.NamespaceId));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "userid", _result.UserId));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "profileid", _result.ProfileId));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "expiretime", ClientInfo.ExpireTime));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "profilenick", _result.ProfileNick));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "uniquenick", _result.UniqueNick));

            certElement.Add(new XElement(SoapXElement.AuthNamespace + "cdkeyhash", _result.CdKeyHash));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "peerkeymodulus", ClientInfo.ModulusHexStr));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "peerkeyexponent", ClientInfo.ExponentHexStr));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "serverdata", ClientInfo.ServerData));
            using (var md5 = MD5.Create())
            {
                var bytes = Encoding.ASCII.GetBytes(context.Value);
                var hash = md5.ComputeHash(bytes);
                var hashString = hash.ToString().Replace("-", string.Empty);
                var enc = EncSignature(hashString);
                var reversedSigStr = enc.ToByteArray().Reverse().ToArray().ToString().Replace("-", string.Empty);
                certElement.Add(new XElement(SoapXElement.AuthNamespace + "signature", reversedSigStr));
            }
            context.Add(certElement);
            context.Add(new XElement(SoapXElement.AuthNamespace + "peerkeyprivate", ClientInfo.PeerKeyPrivateExponent));
        }

        public static BigInteger EncSignature(string hashString) => EncSignature(BigInteger.Parse(ClientInfo.SignaturePreFix + hashString, System.Globalization.NumberStyles.AllowHexSpecifier));
        public static BigInteger EncSignature(BigInteger data) => BigInteger.ModPow(data, ClientInfo.PeerKeyPrivateExponent, ClientInfo.PeerKeyModulus);
    }
}