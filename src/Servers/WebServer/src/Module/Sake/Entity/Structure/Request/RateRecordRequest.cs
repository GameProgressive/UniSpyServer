using System.Linq;

using UniSpyServer.Servers.WebServer.Module.Sake.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request
{
    
    public class RateRecordRequest : RequestBase
    {
        public string RecordId { get; set; }
        public string Rating { get; set; }
        public RateRecordRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var recordid = _contentElement.Descendants().Where(p => p.Name.LocalName == "recordid").First().Value;
            RecordId = recordid;
            var rating = _contentElement.Descendants().Where(p => p.Name.LocalName == "rating").First().Value;
            Rating = rating;
        }
    }
}