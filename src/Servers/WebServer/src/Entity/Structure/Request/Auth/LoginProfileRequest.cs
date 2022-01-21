using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Abstraction;
using UniSpyServer.Servers.WebServer.Entity.Contract;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Request.Auth
{
    [RequestContract("LoginProfile")]
    public class LoginProfileRequest : RequestBase
    {
        public int Version { get; set; }
        public int GameId { get; set; }
        public int PartnerCode { get; set; }
        public int NamespaceId { get; set; }
        public string Email { get; set; }
        public string Uniquenick { get; set; }
        public string CDKey { get; set; }
        public List<FieldObject> Password { get; set; }
        public LoginProfileRequest(string rawRequest) : base(rawRequest)
        {
            Password = new List<FieldObject>();
        }

        public override void Parse()
        {
            var version = _contentElement.Descendants().Where(p => p.Name.LocalName == "version").First().Value;
            Version = int.Parse(version);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = int.Parse(gameid);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = int.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().Where(p => p.Name.LocalName == "namespaceid").First().Value;
            NamespaceId = int.Parse(namespaceid);
            var email = _contentElement.Descendants().Where(p => p.Name.LocalName == "email").First().Value;
            Email = email;
            var uniquenick = _contentElement.Descendants().Where(p => p.Name.LocalName == "uniquenick").First().Value;
            Uniquenick = uniquenick;
            var cdkey = _contentElement.Descendants().Where(p => p.Name.LocalName == "cdkey").First().Value;
            CDKey = cdkey;
            var passwordNode = _contentElement.Descendants().Where(p => p.Name.LocalName == "password").First();
            foreach (XElement element in passwordNode.Nodes())
            {
                Password.Add(new FieldObject(element.Value, element.Name.LocalName));
            }
        }
    }
}