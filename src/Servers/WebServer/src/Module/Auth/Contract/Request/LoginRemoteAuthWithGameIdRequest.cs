using System.Linq;


namespace UniSpy.Server.WebServer.Module.Auth.Contract.Request
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
                throw new Auth.Exception("gameid is missing from the request");
            }
            var gameid = _contentElement.Descendants().FirstOrDefault(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);
        }
    }
}