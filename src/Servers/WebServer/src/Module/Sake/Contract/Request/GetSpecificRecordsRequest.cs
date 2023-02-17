using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpy.Server.WebServer.Contract;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Request
{
    
    public class GetSpecificRecordsRequest : RequestBase
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