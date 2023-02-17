using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpy.Server.WebServer.Entity.Structure;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Structure.Request
{
    
    public class CreateRecordRequest : RequestBase
    {
        public List<FieldObject> Values { get; set; }
        public CreateRecordRequest(string rawRequest) : base(rawRequest)
        {
            Values = new List<FieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
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