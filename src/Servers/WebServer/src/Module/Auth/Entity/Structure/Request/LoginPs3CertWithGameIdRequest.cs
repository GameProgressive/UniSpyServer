using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Exception;

namespace UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Request
{
    
    public sealed class LoginPs3CertWithGameIdRequest : LoginPs3CertRequest
    {
        public int? GameId { get; private set; }
        public LoginPs3CertWithGameIdRequest(string rawRequest) : base(rawRequest)
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