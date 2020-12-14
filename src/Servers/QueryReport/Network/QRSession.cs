using UniSpyLib.Network;
using System.Net;

namespace QueryReport.Network
{
    public class QRSession : UDPSessionBase
    {
        public int InstantKey { get; set; }
        public QRSession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
