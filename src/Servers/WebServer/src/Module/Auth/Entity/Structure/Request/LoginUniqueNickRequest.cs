using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Exception;

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
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "uniquenick"))
            {
                throw new AuthException("uniquenick is missing from the request");
            }
            Uniquenick = _contentElement.Descendants().First(p => p.Name.LocalName == "uniquenick").Value;
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "password"))
            {
                throw new AuthException("password is missing from the request");
            }
            Password = _contentElement.Descendants().First(p => p.Name.LocalName == "password").Value;
        }
    }
}