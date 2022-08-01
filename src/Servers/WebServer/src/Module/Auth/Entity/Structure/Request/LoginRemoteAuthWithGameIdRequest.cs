using System.Linq;

using UniSpyServer.Servers.WebServer.Module.Auth.Exception;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    
    public class LoginRemoteAuthWithGameIdRequest : LoginRemoteAuthRequest
    {
        public int? GameId { get; private set; }
        public LoginRemoteAuthWithGameIdRequest(string rawRequest) : base(rawRequest)
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