using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Exception;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginProfileWithGameId")]
    public sealed class LoginProfileWithGameIdRequest : LoginProfileRequest
    {
        public int GameId { get; private set; }
        public LoginProfileWithGameIdRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "gameid"))
            {
                throw new AuthException("gameid is missing from the request");
            }
            var gameid = _contentElement.Descendants().FirstOrDefault(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);
        }
    }
}