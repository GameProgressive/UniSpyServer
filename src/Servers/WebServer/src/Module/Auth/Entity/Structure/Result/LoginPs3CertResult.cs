using UniSpy.Server.WebServer.Module.Auth.Abstraction;

namespace UniSpy.Server.WebServer.Module.Auth.Entity.Structure.Result
{
    public class LoginPs3CertResult : LoginResultBase
    {
        public string AuthToken { get; set; }
        public string PartnerChallenge { get; set; }
        public LoginPs3CertResult()
        {
        }
    }
}