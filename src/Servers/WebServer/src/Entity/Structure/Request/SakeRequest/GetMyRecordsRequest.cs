using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("GetMyRecords")]
    public class GetMyRecordsRequest : SakeRequestBase
    {
        public List<FieldObject> Fields { get; set; }
        public GetMyRecordsRequest(string rawRequest) : base(rawRequest)
        {
            Fields = new List<FieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
            var fieldsNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "fields").First();
            foreach (XElement element in fieldsNode.Nodes())
            {
                Fields.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
        }
    }
}