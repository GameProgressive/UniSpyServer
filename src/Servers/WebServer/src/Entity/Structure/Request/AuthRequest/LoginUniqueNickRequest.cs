using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WebServer.Abstraction;
using WebServer.Entity.Contract;

namespace WebServer.Entity.Structure.Request.AuthRequest
{
    [RequestContract("LoginUniqueNick")]
    public class LoginUniqueNickRequest : RequestBase
    {
        public uint Version { get; set; }
        public uint PartnerCode { get; set; }
        public uint NamespaceId { get; set; }
        public string Uniquenick { get; set; }
        public List<FieldsObject> Password { get; set; }
        public LoginUniqueNickRequest(string rawRequest) : base(rawRequest)
        {
            Password = new List<FieldsObject>();
        }

        public override void Parse()
        {
            var version = _contentElement.Descendants().Where(p => p.Name.LocalName == "version").First().Value;
            Version = uint.Parse(version);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = uint.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().Where(p => p.Name.LocalName == "namespaceid").First().Value;
            NamespaceId = uint.Parse(namespaceid);
            var uniquenick = _contentElement.Descendants().Where(p => p.Name.LocalName == "uniquenick").First().Value;
            Uniquenick = uniquenick;
            var passwordNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "password").First();
            foreach (XElement element in passwordNode.Nodes())
            {
                Password.Add(new FieldsObject(element.Value, element.Name.LocalName));
            }
        }
    }
}