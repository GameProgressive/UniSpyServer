using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebServer.Abstraction;
using WebServer.Entity.Contract;

namespace WebServer.Entity.Structure.Request.SakeRequest
{
    [RequestContract("SearchForRecords")]
    public class SearchForRecordsRequest : RequestBase
    {
        public uint GameId { get; set; }
        public string SecretKey { get; set; }
        public string LoginTicket { get; set; }
        public string TableId { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public string Offset { get; set; }
        public string Max { get; set; }
        public string Surrounding { get; set; }
        public string OwnerIds { get; set; }
        public string CacheFlag { get; set; }

        public List<FieldsObject> Fields { get; set; }
        public SearchForRecordsRequest(string rawRequest) : base(rawRequest)
        {
            Fields = new List<FieldsObject>();
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
            var filter = _contentElement.Descendants().Where(p => p.Name.LocalName == "filter").First().Value;
            Filter = filter;
            var sort = _contentElement.Descendants().Where(p => p.Name.LocalName == "sort").First().Value;
            Sort = sort;
            var offset = _contentElement.Descendants().Where(p => p.Name.LocalName == "offset").First().Value;
            Offset = offset;
            var max = _contentElement.Descendants().Where(p => p.Name.LocalName == "max").First().Value;
            Max = max;
            var surrounding = _contentElement.Descendants().Where(p => p.Name.LocalName == "surrounding").First().Value;
            Surrounding = surrounding;
            var ownerids = _contentElement.Descendants().Where(p => p.Name.LocalName == "ownerids").First().Value;
            OwnerIds = ownerids;
            var cacheFlag = _contentElement.Descendants().Where(p => p.Name.LocalName == "cacheFlag").First().Value;
            CacheFlag = cacheFlag;
            var fieldsNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "fields").First();
            foreach (XElement element in fieldsNode.Nodes())
            {
                Fields.Add(new FieldsObject(element.Value, element.Name.LocalName));
            }
        }
    }
}