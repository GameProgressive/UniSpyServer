using System;
using System.Linq;
using UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Entity.Structure.Request
{
    
    public class GetStoreAvailabilityRequest : RequestBase
    {
        public int GameId { get; private set; }
        public int Version { get; private set; }
        public string Region { get; private set; }
        public string AccessToken { get; private set; }
        
        public GetStoreAvailabilityRequest(string rawRequest) : base(rawRequest)
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
            
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "version"))
            {
                throw new Exception("version is missing from the request");
            }
            var version = _contentElement.Descendants().First(p => p.Name.LocalName == "version").Value;
            Version = int.Parse(version);

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "region"))
            {
                throw new Exception("region is missing from the request");
            }
            Region = _contentElement.Descendants().First(p => p.Name.LocalName == "region").Value;
            
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "accesstoken"))
            {
                throw new Exception("accesstoken is missing from the request");
            }
            AccessToken = _contentElement.Descendants().First(p => p.Name.LocalName == "accesstoken").Value;
        }
    }
}