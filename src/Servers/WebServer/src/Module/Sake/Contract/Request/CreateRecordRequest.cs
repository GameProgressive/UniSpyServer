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
        }
    }
}