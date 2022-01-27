using System.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Atlas
{
    [RequestContract("SetReportIntention")]
    public class SetReportIntentionRequest : RequestBase
    {
        public string Certificate { get; set; }
        public string Proof { get; set; }
        public int CsId { get; set; }
        public int CcId { get; set; }
        public int GameId { get; set; }
        public string Authoritative { get; set; }
        public SetReportIntentionRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var certificate = _contentElement.Descendants().Where(p => p.Name.LocalName == "certificate").First().Value;
            Certificate = certificate;
            var proof = _contentElement.Descendants().Where(p => p.Name.LocalName == "proof").First().Value;
            Proof = proof;
            var csid = _contentElement.Descendants().Where(p => p.Name.LocalName == "csid").First().Value;
            CsId = int.Parse(csid);
            var ccid = _contentElement.Descendants().Where(p => p.Name.LocalName == "ccid").First().Value;
            CcId = int.Parse(ccid);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var authoritative = _contentElement.Descendants().Where(p => p.Name.LocalName == "authoritative").First().Value;
            Authoritative = authoritative;
        }
    }
}