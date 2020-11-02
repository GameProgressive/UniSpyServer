using UniSpyLib.Network;
using System.Net;

namespace QueryReport.Server
{
    public class QRSession : UDPSessionBase
    {
        public int InstantKey { get; protected set; }
        public QRSession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }

        public void SetInstantKey(int instantKey)
        {
            InstantKey = instantKey;
        }
    }
}
