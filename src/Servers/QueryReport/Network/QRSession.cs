using System.Net;
using UniSpyLib.Abstraction.BaseClass.Network.Udp.Server;

namespace QueryReport.Network
{
    internal sealed class QRSession : UniSpyUdpSession
    {
        public uint InstantKey { get; set; }
        public QRSession(UniSpyUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
