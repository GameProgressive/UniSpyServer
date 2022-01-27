using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Sake
{
    [RequestContract("SearchForRecords")]
    public class SearchForRecordsRequest : Abstraction.Sake.RequestBase
    {
        public string Filter { get; set; }
        public string Sort { get; set; }
        public string Offset { get; set; }
        public string Max { get; set; }
        public string Surrounding { get; set; }
        public string OwnerIds { get; set; }
        public string CacheFlag { get; set; }

        public List<FieldObject> Fields { get; set; }
        public SearchForRecordsRequest(string rawRequest) : base(rawRequest)
        {
            Fields = new List<FieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
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
                Fields.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
        }
    }
}