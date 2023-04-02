using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;


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
                throw new Auth.Exception("email is missing from the request");
            }
            Email = _contentElement.Descendants().First(p => p.Name.LocalName == "email").Value;
            
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "uniquenick"))
            {
                throw new Auth.Exception("uniquenick is missing from the request");
            }
            Uniquenick = _contentElement.Descendants().First(p => p.Name.LocalName == "uniquenick").Value;

            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "cdkey"))
            {
                throw new Auth.Exception("cdkey is missing from the request");
            }
            CDKey = _contentElement.Descendants().First(p => p.Name.LocalName == "cdkey").Value;
            
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "password"))
            {
                throw new Auth.Exception("password is missing from the request");
            }
            Password = _contentElement.Descendants().First(p => p.Name.LocalName == "password").Value;
        }
    }
}