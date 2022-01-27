using System.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth
{
    [RequestContract("LoginRemoteAuth")]
    public class LoginRemoteAuthRequest : RequestBase
    {
        public int Version { get; set; }
        public int GameId { get; set; }
        public int PartnerCode { get; set; }
        public int NamespaceId { get; set; }
        public string AuthToken { get; set; }
        public string Challenge { get; set; }
        public LoginRemoteAuthRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var version = _contentElement.Descendants().Where(p => p.Name.LocalName == "version").First().Value;
            Version = int.Parse(version);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = int.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().Where(p => p.Name.LocalName == "namespaceid").First().Value;
            NamespaceId = int.Parse(namespaceid);
            var authtoken = _contentElement.Descendants().Where(p => p.Name.LocalName == "authtoken").First().Value;
            AuthToken = authtoken;
            var challenge = _contentElement.Descendants().Where(p => p.Name.LocalName == "challenge").First().Value;
            Challenge = challenge;
        }
    }
}