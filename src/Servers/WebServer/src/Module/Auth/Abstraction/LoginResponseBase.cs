using System;
using System.Collections.Generic;
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

            certElement.Add(new XElement(SoapXElement.AuthNamespace + "peerkeymodulus", ClientInfo.PeerKeyPublicModulus));
            certElement.Add(new XElement(SoapXElement.AuthNamespace + "peerkeyexponent", ClientInfo.PeerKeyPrivate));

            certElement.Add(new XElement(SoapXElement.AuthNamespace + "serverdata", ClientInfo.ServerData));
            using (var md5 = MD5.Create())
            {
                var dataToHash = new List<byte>();
                dataToHash.AddRange(BitConverter.GetBytes(_result.Length));
                dataToHash.AddRange(BitConverter.GetBytes(_request.Version));
                dataToHash.AddRange(BitConverter.GetBytes(_request.PartnerCode));
                dataToHash.AddRange(BitConverter.GetBytes(_request.NamespaceId));
                dataToHash.AddRange(BitConverter.GetBytes(_result.UserId));
                dataToHash.AddRange(BitConverter.GetBytes(_result.ProfileId));
                dataToHash.AddRange(BitConverter.GetBytes(ClientInfo.ExpireTime));
                dataToHash.AddRange(Encoding.ASCII.GetBytes(_result.ProfileNick));
                dataToHash.AddRange(Encoding.ASCII.GetBytes(_result.UniqueNick));
                dataToHash.AddRange(Encoding.ASCII.GetBytes(_result.CdKeyHash));

                // if these 2 value be 0 we do not need to add them to the list
                // dataToHash.AddRange(ClientInfo.PeerKeyPublicModulus.FromHexStringToBytes());
                // dataToHash.AddRange(ClientInfo.PeerKeyPrivate.FromHexStringToBytes());

                // server data should be convert to bytes[128] then added to list
                dataToHash.AddRange(ClientInfo.ServerData.FromHexStringToBytes());
                var hash = md5.ComputeHash(dataToHash.ToArray());
                var hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
                certElement.Add(new XElement(SoapXElement.AuthNamespace + "signature", ClientInfo.SignaturePreFix + hashString));
            }
            context.Add(certElement);
            context.Add(new XElement(SoapXElement.AuthNamespace + "peerkeyprivate", ClientInfo.PeerKeyPrivate));
        }
    }
}