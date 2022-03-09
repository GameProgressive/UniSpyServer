using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Entity.Structure;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginProfile")]
    public class LoginProfileRequest : LoginRequestBase
    {

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
            base.Parse();
            
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