using GameSpyLib.Network;
using System.Net;

namespace QueryReport.Server
{
    public class QRSession : TemplateUdpSession
    {
        public QRSession(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
