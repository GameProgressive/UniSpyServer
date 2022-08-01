using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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
        }

        protected void BuildContext()
        {
            _content.Add("responseCode", _result.ResponseCode);
            _content.Add("certificate");
            _content.Add("length", _result.Length);
            _content.Add("version", _request.Version);
            _content.Add("partnercode", _request.PartnerCode);
            _content.Add("namespaceid", _request.NamespaceId);
            _content.Add("userid", _result.UserId);
            _content.Add("profileid", _result.ProfileId);
            _content.Add("expiretime", ClientInfo.ExpireTime);
            _content.Add("profilenick", _result.ProfileNick);
            _content.Add("uniquenick", _result.UniqueNick);
            _content.Add("cdkeyhash", _result.CdKeyHash);
            _content.Add("peerkeymodulus", ClientInfo.PeerKeyPublicModulus);
            _content.Add("peerkeyexponent", ClientInfo.PeerKeyPrivate);
            _content.Add("serverdata", ClientInfo.ServerData);

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
                _content.Add("signature", ClientInfo.SignaturePreFix + hashString);
            }
            _content.BackToParentElement();
            _content.Add("peerkeyprivate", ClientInfo.PeerKeyPrivate);
        }
    }
}