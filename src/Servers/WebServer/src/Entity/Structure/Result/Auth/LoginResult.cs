using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Entity.Structure.Result.Auth
{
    public class LoginResult : ResultBase
    {
        public int ResponseCode { get; set; }
        public string Certificate { get; set; }
        public string PeerKeyPrivate { get; set; }
        public LoginResult()
        {
        }
    }
}