using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;


namespace UniSpy.Server.WebServer.Module.Auth.Contract.Request
{
    
    public class LoginRemoteAuthRequest : LoginRequestBase
    {
        public string AuthToken { get; private set; }
        public string Challenge { get; private set; }
        public LoginRemoteAuthRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "authtoken"))
            {
                throw new Auth.Exception("authtoken is missing from the request");
            }
            AuthToken = _contentElement.Descendants().First(p => p.Name.LocalName == "authtoken").Value;
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "challenge"))
            {
                throw new Auth.Exception("challenge is missing from the request");
            }
            Challenge = _contentElement.Descendants().First(p => p.Name.LocalName == "challenge").Value;
        }
    }
}