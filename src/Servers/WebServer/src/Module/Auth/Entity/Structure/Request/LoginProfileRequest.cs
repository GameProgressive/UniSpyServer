using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginProfileWithGameId")]
    public class LoginProfileRequest : LoginRequestBase
    {
        public int GameId { get; private set; }

        public string Email { get; private set; }
        public string Uniquenick { get; private set; }
        public string CDKey { get; private set; }
        public string Password { get; private set; }
        public LoginProfileRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var gameid = _contentElement.Descendants().FirstOrDefault(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);
            Email = _contentElement.Descendants().First(p => p.Name.LocalName == "email").Value;
            Uniquenick = _contentElement.Descendants().First(p => p.Name.LocalName == "uniquenick").Value;
            CDKey = _contentElement.Descendants().First(p => p.Name.LocalName == "cdkey").Value;
            Password = _contentElement.Descendants().First(p => p.Name.LocalName == "password").Value;
        }
    }
}