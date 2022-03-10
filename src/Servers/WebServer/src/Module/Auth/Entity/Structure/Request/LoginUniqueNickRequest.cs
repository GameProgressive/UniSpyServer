using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginUniqueNick")]
    public class LoginUniqueNickRequest : LoginRequestBase
    {
        public string Uniquenick { get; private set; }
        public string Password { get; private set; }
        public LoginUniqueNickRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            Uniquenick = _contentElement.Descendants().First(p => p.Name.LocalName == "uniquenick").Value;
            Password = _contentElement.Descendants().First(p => p.Name.LocalName == "password").Value;
        }
    }
}