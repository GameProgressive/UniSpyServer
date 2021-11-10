using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.RacingRequest
{
    [RequestContract("SubmitGhost")]
    public class SubmitGhostRequest : RequestBase
    {
        public uint GameId { get; set; }
        public uint RegionId { get; set; }
        public uint CourseId { get; set; }
        public uint ProfileId { get; set; }
        public string Score { get; set; }
        public uint FileId { get; set; }
        public SubmitGhostRequest(string rawRequest) : base(rawRequest)
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
            var profileid = _contentElement.Descendants().Where(p => p.Name.LocalName == "profileid").First().Value;
            ProfileId = uint.Parse(profileid);
            var score = _contentElement.Descendants().Where(p => p.Name.LocalName == "score").First().Value;
            Score = score;
            var fileid = _contentElement.Descendants().Where(p => p.Name.LocalName == "fileid").First().Value;
            FileId = uint.Parse(fileid);
        }
    }
}