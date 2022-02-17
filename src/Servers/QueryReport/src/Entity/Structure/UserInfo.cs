using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Entity
{
    public class ClientInfo : ClientInfoBase
    {
        public uint? InstantKey { get; set; }
        public ClientInfo(IPEndPoint remoteIPEndPoint) : base(remoteIPEndPoint)
        {
        }
    }
}