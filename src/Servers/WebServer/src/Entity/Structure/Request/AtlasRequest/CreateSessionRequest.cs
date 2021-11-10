using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.AtlasRequest
{
    [RequestContract("CreateSession")]
    public class CreateSessionRequest : RequestBase
    {
        public string Certificate { get; set; }
        public string Proof { get; set; }
        public uint GameId { get; set; }
        public CreateSessionRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var certificate = _contentElement.Descendants().Where(p => p.Name.LocalName == "certificate").First().Value;
            Certificate = certificate;
            var proof = _contentElement.Descendants().Where(p => p.Name.LocalName == "proof").First().Value;
            Proof = proof;
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameid);
        }
    }
}