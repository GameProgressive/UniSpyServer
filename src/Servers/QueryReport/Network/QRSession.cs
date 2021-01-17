using System.Net;
using UniSpyLib.Network;

namespace QueryReport.Network
{
    public class QRSession : UniSpyUDPSessionBase
    {
        public int InstantKey { get; set; }
        public QRSession(UniSpyUDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
