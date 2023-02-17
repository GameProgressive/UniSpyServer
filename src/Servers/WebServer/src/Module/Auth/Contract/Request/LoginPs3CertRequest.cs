using System.Linq;
using UniSpy.Server.WebServer.Module.Auth.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Exception;

namespace UniSpy.Server.WebServer.Module.Auth.Contract.Request
{
    
    public class LoginPs3CertRequest : LoginRequestBase
    {
        public string PS3cert { get; private set; }
        public LoginPs3CertRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "npticket"))
            {
                throw new AuthException("ps3cert is missing from the request");
            }
            PS3cert = _contentElement.Descendants().First(p => p.Name.LocalName == "npticket").Value;
        }
    }
}