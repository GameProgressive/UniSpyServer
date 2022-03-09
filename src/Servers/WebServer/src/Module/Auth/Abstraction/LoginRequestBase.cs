using System.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Abstraction
{
    public abstract class LoginRequestBase : RequestBase
    {
        protected LoginRequestBase(string rawRequest) : base(rawRequest)
        {
        }

        public int Version { get; set; }
        public int GameId { get; set; }
        public int PartnerCode { get; set; }
        public int NamespaceId { get; set; }
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
        }
    }
}