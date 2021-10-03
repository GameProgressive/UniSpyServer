using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WebServer.Abstraction;
using WebServer.Entity.Contract;

namespace WebServer.Entity.Structure.Request
{
    [RequestContract("GetMyRecords")]
    public class GetMyRecordRequest : RequestBase
    {
        public uint GameId { get; set; }
        public string SecretKey { get; set; }
        public string LoginTicket { get; set; }
        public string TableId { get; set; }
        public List<FieldsObject> Fields { get; set; }
        public GetMyRecordRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            dynamic xelements = XElement.Parse(RawRequest);
            XElement contentNode = xelements.FirstNode.FirstNode;
            var gameId = contentNode.Descendants().Where(p => p.Name.LocalName == "gameid").First<XElement>().Value;
            GameId = uint.Parse(gameId);
            var secretKey = contentNode.Descendants().Where(p => p.Name.LocalName == "secretKey").First<XElement>().Value;
            SecretKey = secretKey;
            var loginTicket = contentNode.Descendants().Where(p => p.Name.LocalName == "loginTicket").First<XElement>().Value;
            LoginTicket = loginTicket;
            var tableid = contentNode.Descendants().Where(p => p.Name.LocalName == "tableid").First<XElement>().Value;
            TableId = tableid;
            var fieldsNode = contentNode.Descendants().Where(p => p.Name.LocalName == "fields").First<XElement>();
            var fields = new List<FieldsObject>();
            foreach (XElement element in fieldsNode.Nodes())
            {
                fields.Add(new FieldsObject(element.Value, element.Name.LocalName));
            }
        }
    }
}