using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Racing
{
    [RequestContract("SubmitScores")]
    public class SubmitScoresRequest : RequestBase
    {
        public int GameData { get; set; }
        public int RegionId { get; set; }
        public int ProfileId { get; set; }
        public int GameId { get; set; }
        public int ScoreMode { get; set; }
        public string ScoreDatas { get; set; }
        public SubmitScoresRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var gamedata = _contentElement.Descendants().Where(p => p.Name.LocalName == "gamedata").First().Value;
            GameData = int.Parse(gamedata);
            var regionid = _contentElement.Descendants().Where(p => p.Name.LocalName == "regionid").First().Value;
            RegionId = int.Parse(regionid);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var scoremode = _contentElement.Descendants().Where(p => p.Name.LocalName == "scoremode").First().Value;
            ScoreMode = int.Parse(scoremode);
            var scoredatas = _contentElement.Descendants().Where(p => p.Name.LocalName == "scoredatas").First().Value;
            ScoreDatas = scoredatas;
        }
    }
}