using System.Linq;
using UniSpyServer.Servers.WebServer.Entity.Contract;
using UniSpyServer.Servers.WebServer.Module.Auth.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Auth.Entity.Structure.Request
{
    [RequestContract("LoginPs3CertWithGameId")]
    public class LoginPs3CertRequest : LoginRequestBase
    {
        public int? GameId { get; private set; }
        public string PS3cert { get; private set; }
        public LoginPs3CertRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            var gameid = _contentElement.Descendants().FirstOrDefault(p => p.Name.LocalName == "gameid").Value;
            GameId = int.Parse(gameid);
            PS3cert = _contentElement.Descendants().First(p => p.Name.LocalName == "npticket").Value;
        }
    }
}