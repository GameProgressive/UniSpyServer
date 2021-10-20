using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.WebServer.Abstraction;
using UniSpyServer.WebServer.Entity.Contract;

namespace UniSpyServer.WebServer.Entity.Structure.Request.AuthRequest
{
    [RequestContract("LoginProfile")]
    public class LoginProfileRequest : RequestBase
    {
        public uint Version { get; set; }
        public uint GameId { get; set; }
        public uint PartnerCode { get; set; }
        public uint NamespaceId { get; set; }
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
            Version = uint.Parse(version);
            var gameid = _contentElement.Descendants().Where(p => p.Name.LocalName == "gameid").First().Value;
            GameId = uint.Parse(gameid);
            var partnercode = _contentElement.Descendants().Where(p => p.Name.LocalName == "partnercode").First().Value;
            PartnerCode = uint.Parse(partnercode);
            var namespaceid = _contentElement.Descendants().Where(p => p.Name.LocalName == "namespaceid").First().Value;
            NamespaceId = uint.Parse(namespaceid);
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