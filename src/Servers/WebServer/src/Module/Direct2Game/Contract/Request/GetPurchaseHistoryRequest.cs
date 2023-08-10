using System.Linq;
using UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Contract.Request
{

    public class GetPurchaseHistoryRequest : RequestBase
    {
        public int GameId { get; private set; }

        public string AccessToken { get; private set; }

        public string Proof { get; private set; }
        public string Certificate { get; private set; }
        public GetPurchaseHistoryRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "gameid"))
            {
                throw new WebServer.Exception("gameid is missing from the request");
            }
            var gameid = _contentElement.Descendants().First(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "proof"))
            {
                throw new WebServer.Exception("proof is missing from the request");
            }
            Proof = _contentElement.Descendants().First(p => p.Name.LocalName == "proof").Value;
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "certificate"))
            {
                throw new WebServer.Exception("certificate is missing from the request");
            }
            Certificate = _contentElement.Descendants().First(p => p.Name.LocalName == "certificate").Value;
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "accesstoken"))
            {
                throw new WebServer.Exception("accesstoken is missing from the request");
            }
            AccessToken = _contentElement.Descendants().First(p => p.Name.LocalName == "accesstoken").Value;
        }

    }
}
