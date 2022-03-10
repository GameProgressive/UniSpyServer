using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginRemoteAuthWithGameId")]
    public class LoginRemoteAuthRequest : LoginRequestBase
    {
        public int? GameId { get; private set; }
        public string AuthToken { get; private set; }
        public string Challenge { get; private set; }
        public LoginRemoteAuthRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var gameid = _contentElement.Descendants().FirstOrDefault(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);
            AuthToken = _contentElement.Descendants().First(p => p.Name.LocalName == "authtoken").Value;
            Challenge = _contentElement.Descendants().First(p => p.Name.LocalName == "challenge").Value;
        }
    }
}