using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Sake.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request
{
    [RequestContract("DeleteRecord")]
    public class DeleteRecordRequest : RequestBase
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