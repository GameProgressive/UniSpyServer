using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.RacingRequest
{
    [RequestContract("GetTopTenRankings")]
    public class GetTopTenRankingsRequest : RequestBase
    {
        public uint GameId { get; set; }
        public uint RegionId { get; set; }
        public uint CourseId { get; set; }
        public GetTopTenRankingsRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameid);
            var regionid = _contentElement.Descendants().Where(p => p.Name.LocalName == "regionid").First().Value;
            RegionId = uint.Parse(regionid);
            var courseid = _contentElement.Descendants().Where(p => p.Name.LocalName == "courseid").First().Value;
            CourseId = uint.Parse(courseid);
        }
    }
}