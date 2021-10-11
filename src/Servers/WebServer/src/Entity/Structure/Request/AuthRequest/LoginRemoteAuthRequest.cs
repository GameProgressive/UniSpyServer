using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebServer.Abstraction;
using WebServer.Entity.Contract;

namespace WebServer.Entity.Structure.Request.AuthRequest
{
    [RequestContract("LoginRemoteAuth")]
    public class LoginRemoteAuthRequest : RequestBase
    {
        public uint Version { get; set; }
        public uint GameId { get; set; }
        public uint PartnerCode { get; set; }
        public uint NamespaceId { get; set; }
        public string AuthToken { get; set; }
        public string Challenge { get; set; }
        public LoginRemoteAuthRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var version = _contentElement.Descendants().Where(p => p.Name.LocalName == "version").First().Value;
            Version = uint.Parse(version);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameid);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = uint.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().Where(p => p.Name.LocalName == "namespaceid").First().Value;
            NamespaceId = uint.Parse(namespaceid);
            var authtoken = _contentElement.Descendants().Where(p => p.Name.LocalName == "authtoken").First().Value;
            AuthToken = authtoken;
            var challenge = _contentElement.Descendants().Where(p => p.Name.LocalName == "challenge").First().Value;
            Challenge = challenge;
        }
    }
}