using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace CDKey.Network
{
    internal sealed class CDKeySession : UniSpyUdpSession
    {
        public CDKeySession(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
