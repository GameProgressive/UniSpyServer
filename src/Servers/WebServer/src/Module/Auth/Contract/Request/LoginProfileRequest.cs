using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Exception;

namespace UniSpy.Server.WebServer.Module.Auth.Contract.Request
{
    
    public class LoginProfileRequest : LoginRequestBase
    {
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
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "email"))
            {
                throw new AuthException("email is missing from the request");
            }
            Email = _contentElement.Descendants().First(p => p.Name.LocalName == "email").Value;
            
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "uniquenick"))
            {
                throw new AuthException("uniquenick is missing from the request");
            }
            Uniquenick = _contentElement.Descendants().First(p => p.Name.LocalName == "uniquenick").Value;

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "cdkey"))
            {
                throw new AuthException("cdkey is missing from the request");
            }
            CDKey = _contentElement.Descendants().First(p => p.Name.LocalName == "cdkey").Value;
            
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "password"))
            {
                throw new AuthException("password is missing from the request");
            }
            Password = _contentElement.Descendants().First(p => p.Name.LocalName == "password").Value;
        }
    }
}