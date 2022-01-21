using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth
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