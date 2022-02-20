using System.Net;
using UniSpyServer.Servers.ServerBrowser.Abstraction;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        public string GameSecretKey { get; set; }
        public string ClientChallenge { get; set; }
        public AdHocRequest AdHocMessage { get; set; }
        public ClientInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }
    }
}