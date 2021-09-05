using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace NatNegotiation.Network
{
    internal sealed class Session : UniSpyUdpSession
    {
        public Session(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
