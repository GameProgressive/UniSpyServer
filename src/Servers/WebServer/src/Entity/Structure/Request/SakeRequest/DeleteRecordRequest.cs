using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("DeleteRecord")]
    public class DeleteRecordRequest : SakeRequestBase
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