using GameSpyLib.Network;
using System.Net;

namespace QueryReport.Server
{
    public class QRClient : TemplateUdpClient
    {
        public QRClient(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
