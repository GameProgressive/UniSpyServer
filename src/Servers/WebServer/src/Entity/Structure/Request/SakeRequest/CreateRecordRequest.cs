using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebServer.Abstraction;
using WebServer.Entity.Contract;

namespace WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("CreateRecord")]
    public class CreateRecordRequest : RequestBase
    {
        public uint GameId { get; set; }
        public string SecretKey { get; set; }
        public string LoginTicket { get; set; }
        public string TableId { get; set; }

        public List<FieldObject> Values { get; set; }
        public CreateRecordRequest(string rawRequest) : base(rawRequest)
        {
            Values = new List<FieldObject>();
        }

        public override void Parse()
        {
            var gameId = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameId);
            var secretKey = _contentElement.Descendants().Where(p => p.Name.LocalName == "secretKey").First().Value;
            SecretKey = secretKey;
            var loginTicket = _contentElement.Descendants().Where(p => p.Name.LocalName == "loginTicket").First().Value;
            LoginTicket = loginTicket;
            var tableid = _contentElement.Descendants().Where(p => p.Name.LocalName == "tableid").First().Value;
            TableId = tableid;
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