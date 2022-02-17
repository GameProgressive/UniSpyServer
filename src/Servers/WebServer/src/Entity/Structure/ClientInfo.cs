using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.WebServer.Entity.Structure
{
    public class ClientInfo : ClientInfoBase
    {
        public ClientInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }
    }
}