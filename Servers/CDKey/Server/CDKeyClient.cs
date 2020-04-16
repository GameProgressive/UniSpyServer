using System;
using System.Net;
using GameSpyLib.Network;

namespace CDKey.Server
{
    public class CDKeyClient : TemplateUdpClient
    {
        public CDKeyClient(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
