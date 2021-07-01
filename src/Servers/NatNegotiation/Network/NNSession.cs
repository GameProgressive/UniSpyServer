using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace NatNegotiation.Network
{
    internal sealed class NNSession : UniSpyUdpSession
    {
        public NNSession(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
