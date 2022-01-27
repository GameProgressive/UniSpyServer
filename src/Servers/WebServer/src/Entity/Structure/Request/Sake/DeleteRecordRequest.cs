using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake
{
    [RequestContract("DeleteRecord")]
    public class DeleteRecordRequest : Abstraction.Sake.RequestBase
    {
        public string RecordId { get; set; }
        public DeleteRecordRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var recordid = _contentElement.Descendants().Where(p => p.Name.LocalName == "recordid").First().Value;
            RecordId = recordid;
        }
    }
}