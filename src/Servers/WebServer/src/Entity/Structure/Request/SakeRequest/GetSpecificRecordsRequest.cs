using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("GetSpecificRecords")]
    public class GetSpecificRecordsRequest : SakeRequestBase
    {
        public List<FieldObject> RecordIds { get; set; }
        public List<FieldObject> Fields { get; set; }
        public GetSpecificRecordsRequest(string rawRequest) : base(rawRequest)
        {
            RecordIds = new List<FieldObject>();
            Fields = new List<FieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
            var recordidsNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "recordids").First();
            foreach (XElement element in recordidsNode.Nodes())
            {
                RecordIds.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
            var fieldsNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "fields").First();
            foreach (XElement element in fieldsNode.Nodes())
            {
                Fields.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
        }
    }
}