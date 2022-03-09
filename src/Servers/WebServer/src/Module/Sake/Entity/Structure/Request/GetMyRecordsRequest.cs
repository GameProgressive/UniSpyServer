using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Sake.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Structure.Request
{
    [RequestContract("GetMyRecords")]
    public class GetMyRecordsRequest : RequestBase
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