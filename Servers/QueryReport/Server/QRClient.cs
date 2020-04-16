using System;
using System.Net;
using GameSpyLib.Network;

namespace QueryReport.Server
{
    public class QRClient : TemplateUdpClient
    {
        public QRClient(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
