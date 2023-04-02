using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;


namespace UniSpy.Server.WebServer.Module.Auth.Contract.Request
{
    
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
                throw new Auth.Exception("uniquenick is missing from the request");
            }
            Uniquenick = _contentElement.Descendants().First(p => p.Name.LocalName == "uniquenick").Value;
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "password"))
            {
                throw new Auth.Exception("password is missing from the request");
            }
            Password = _contentElement.Descendants().First(p => p.Name.LocalName == "password").Value;
        }
    }
}