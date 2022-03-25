using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;
using UniSpyServer.Servers.WebServer.Module.Auth.Exception;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginPs3Cert")]
    public class LoginPs3CertRequest : LoginRequestBase
    {
        public string PS3cert { get; private set; }
        public LoginPs3CertRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if (!_contentElement.Descendants().Any(p => p.Name.LocalName == "ps3cert"))
            {
                throw new AuthException("ps3cert is missing from the request");
            }
            PS3cert = _contentElement.Descendants().First(p => p.Name.LocalName == "npticket").Value;
        }
    }
}