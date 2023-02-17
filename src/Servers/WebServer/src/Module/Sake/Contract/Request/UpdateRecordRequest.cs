using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpy.Server.WebServer.Contract;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Request
{
    
    public class UpdateRecordRequest : RequestBase
    {
        public string RecordId { get; set; }

        public List<RecordFieldObject> Values { get; set; }
        public UpdateRecordRequest(string rawRequest) : base(rawRequest)
        {
            Values = new List<RecordFieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
            var recordid = _contentElement.Descendants().Where(p => p.Name.LocalName == "recordid").First().Value;
            RecordId = recordid;
            var valuesNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "values").First();
            foreach (XElement element in valuesNode.Nodes())
            {
                // TODO move this code to RecordFieldObject as static method which takes XElement as input returns a List<RecordFieldObject>
                // first we find the value name by string "name"
                var name = element.Descendants().Where(p => p.Name.LocalName == "name").First().Value;
                // then we get the value type by string "value"
                var type = element.Descendants().Where(p => p.Name.LocalName == "value").First().Descendants().First().Name.LocalName;
                // then we get the actual value by its type we get before
                var value = element.Descendants().Where(p => p.Name.LocalName == type).First().Value;
                Values.Add(new RecordFieldObject(value, name, type));
            }
        }
    }
}