using System.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Racing
{
    [RequestContract("GetContestData")]
    public class GetContestDataRequest : RequestBase
    {
        public int GameId { get; set; }
        public int RegionId { get; set; }
        public int CourseId { get; set; }
        public GetContestDataRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var regionid = _contentElement.Descendants().Where(p => p.Name.LocalName == "regionid").First().Value;
            RegionId = int.Parse(regionid);
            var courseid = _contentElement.Descendants().Where(p => p.Name.LocalName == "courseid").First().Value;
            CourseId = int.Parse(courseid);
        }
    }
}