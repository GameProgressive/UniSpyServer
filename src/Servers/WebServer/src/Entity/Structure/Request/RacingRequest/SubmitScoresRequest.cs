using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebServer.Abstraction;
using WebServer.Entity.Contract;

namespace WebServer.Entity.Structure.Request.RacingRequest
{
    [RequestContract("SubmitScores")]
    public class SubmitScoresRequest : RequestBase
    {
        public uint GameData { get; set; }
        public uint RegionId { get; set; }
        public uint ProfileId { get; set; }
        public uint GameId { get; set; }
        public uint ScoreMode { get; set; }
        public string ScoreDatas { get; set; }
        public SubmitScoresRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var gamedata = _contentElement.Descendants().Where(p => p.Name.LocalName == "gamedata").First().Value;
            GameData = uint.Parse(gamedata);
            var regionid = _contentElement.Descendants().Where(p => p.Name.LocalName == "regionid").First().Value;
            RegionId = uint.Parse(regionid);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameid);
            var scoremode = _contentElement.Descendants().Where(p => p.Name.LocalName == "scoremode").First().Value;
            ScoreMode = uint.Parse(scoremode);
            var scoredatas = _contentElement.Descendants().Where(p => p.Name.LocalName == "scoredatas").First().Value;
            ScoreDatas = scoredatas;
        }
    }
}