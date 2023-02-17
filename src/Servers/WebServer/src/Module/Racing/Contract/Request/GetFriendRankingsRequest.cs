using System.Linq;
using UniSpy.Server.WebServer.Abstraction;


namespace UniSpy.Server.WebServer.Module.Racing.Contract.Request
{
    
    public class GetFriendRankingsRequest : RequestBase
    {
        public int GameId { get; set; }
        public int RegionId { get; set; }
        public int CourseId { get; set; }
        public int ProfileId { get; set; }
        public GetFriendRankingsRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var regionid = _contentElement.Descendants().Where(p => p.Name.LocalName == "regionid").First().Value;
            RegionId = int.Parse(regionid);
            var courseid = _contentElement.Descendants().Where(p => p.Name.LocalName == "courseid").First().Value;
            CourseId = int.Parse(courseid);
            var profileid = _contentElement.Descendants().Where(p => p.Name.LocalName == "profileid").First().Value;
            ProfileId = int.Parse(profileid);
        }
    }
}