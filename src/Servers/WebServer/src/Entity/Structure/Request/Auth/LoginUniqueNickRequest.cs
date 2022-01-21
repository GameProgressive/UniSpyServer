using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth
{
    [RequestContract("LoginUniqueNick")]
    public class LoginUniqueNickRequest : RequestBase
    {
        public int Version { get; set; }
        public int PartnerCode { get; set; }
        public int NamespaceId { get; set; }
        public string Uniquenick { get; set; }
        public List<FieldObject> Password { get; set; }
        public LoginUniqueNickRequest(string rawRequest) : base(rawRequest)
        {
            Password = new List<FieldObject>();
        }

        public override void Parse()
        {
            base.Parse();
            var version = _contentElement.Descendants().Where(p => p.Name.LocalName == "version").First().Value;
            Version = int.Parse(version);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = int.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().Where(p => p.Name.LocalName == "namespaceid").First().Value;
            NamespaceId = int.Parse(namespaceid);
            var uniquenick = _contentElement.Descendants().Where(p => p.Name.LocalName == "uniquenick").First().Value;
            Uniquenick = uniquenick;
            var passwordNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "password").First();
            foreach (XElement element in passwordNode.Nodes())
            {
                Password.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
        }
    }
}