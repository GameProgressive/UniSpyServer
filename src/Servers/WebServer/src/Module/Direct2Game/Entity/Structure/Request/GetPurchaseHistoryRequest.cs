using System;
using System.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;


namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure.Request
{
    
    public class GetPurchaseHistoryRequest : RequestBase
    {
        public int GameId { get; private set; }

        public string AccessToken { get; private set; }

        public string Proof { get; private set; }

        public GetPurchaseHistoryRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "gameid"))
            {
                throw new Exception("gameid is missing from the request");
            }
            var gameid = _contentElement.Descendants().First(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "proof"))
            {
                throw new Exception("proof is missing from the request");
            }
            Proof = _contentElement.Descendants().First(p => p.Name.LocalName == "proof").Value;

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "accesstoken"))
            {
                throw new Exception("accesstoken is missing from the request");
            }
            AccessToken = _contentElement.Descendants().First(p => p.Name.LocalName == "accesstoken").Value;
        }

    }
}
