using GameSpyLib.Network;
using System.Net;

namespace QueryReport.Server
{
    public class QRSession : TemplateUdpSession
    {
        public int InstantKey { get; protected set; }
        public QRSession(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }

        public void SetInstantKey(int instantKey)
        {
            InstantKey = instantKey;
        }
    }
}
