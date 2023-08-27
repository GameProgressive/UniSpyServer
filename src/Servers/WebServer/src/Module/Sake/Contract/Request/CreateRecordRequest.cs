using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;
using UniSpy.Server.WebServer.Aggregate;
using UniSpy.Server.WebServer.Module.Sake.Abstraction;

namespace UniSpy.Server.WebServer.Module.Sake.Contract.Request
{

    public class CreateRecordRequest : RequestBase
    {
        public static readonly List<string> SakeTypes = new List<string>()
        {
            "binaryDataValue",
            "booleanValue",
            "dateAndTimeValue",
            "unicodeStringValue",
            "asciiStringValue",
            "floatValue",
            "int64Value",
            "intValue",
            "shortValue",
            "byteValue"
        };
        public JsonDocument Values { get; set; }
        public CreateRecordRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var valuesNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "ArrayOfRecordValue").First();
            var jStr = Newtonsoft.Json.JsonConvert.SerializeXNode(valuesNode).Replace("ns1:", "");
            Values = JsonDocument.Parse(jStr);
            // foreach (XElement element in valuesNode.Nodes())
            // {
            //     // TODO move this code to RecordFieldObject as static method which takes XElement as input returns a List<RecordFieldObject>
            //     // first we find the value name by string "name"
            //     var name = element.Descendants().Where(p => p.Name.LocalName == "name").First().Value;
            //     // then we get the value type by string "value"
            //     var type = element.Descendants().Where(p => p.Name.LocalName == "value").First().Descendants().First().Name.LocalName;
            //     // then we get the actual value by its type we get before
            //     var value = element.Descendants().Where(p => p.Name.LocalName == type).First().Value;
            //     Values.Add(new RecordFieldObject(value, name, type));
            // }
        }
    }
}