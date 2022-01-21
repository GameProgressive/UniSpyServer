using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth
{
    [RequestContract("LoginPs3Cert")]
    public class LoginPs3CertRequest : RequestBase
    {
        public int GameId { get; set; }
        public int PartnerCode { get; set; }
        public int PS3cert { get; set; }
        public LoginPs3CertRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = int.Parse(partnercode);
            var ps3cert = _contentElement.Descendants().Where(p => p.Name.LocalName == "ps3cert").First().Value;
            PS3cert = int.Parse(ps3cert);
        }
    }
}