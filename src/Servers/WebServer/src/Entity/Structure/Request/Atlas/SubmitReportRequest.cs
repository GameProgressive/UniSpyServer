using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Atlas
{
    [RequestContract("SubmitReport")]
    public class SubmitReportRequest : RequestBase
    {
        public string Certificate { get; set; }
        public string Proof { get; set; }
        public uint CsId { get; set; }
        public uint CcId { get; set; }
        public uint GameId { get; set; }
        public string Authoritative { get; set; }
        public SubmitReportRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var certificate = _contentElement.Descendants().Where(p => p.Name.LocalName == "certificate").First().Value;
            Certificate = certificate;
            var proof = _contentElement.Descendants().Where(p => p.Name.LocalName == "proof").First().Value;
            Proof = proof;
            var csid = _contentElement.Descendants().Where(p => p.Name.LocalName == "csid").First().Value;
            CsId = uint.Parse(csid);
            var ccid = _contentElement.Descendants().Where(p => p.Name.LocalName == "ccid").First().Value;
            CcId = uint.Parse(ccid);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameid);
            var authoritative = _contentElement.Descendants().Where(p => p.Name.LocalName == "authoritative").First().Value;
            Authoritative = authoritative;
        }
    }
}