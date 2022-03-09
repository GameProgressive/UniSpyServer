using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Result
{
    public class LoginPs3CertResult : ResultBase
    {
        public int ResponseCode { get; set; }
        public string AuthToken { get; set; }
        public string PartnerChallenge { get; set; }
        public LoginPs3CertResult()
        {
        }
    }
}