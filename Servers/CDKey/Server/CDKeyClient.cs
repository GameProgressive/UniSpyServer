using GameSpyLib.Network;
using System.Net;

namespace CDKey.Server
{
    public class CDKeyClient : TemplateUdpClient
    {
        public CDKeyClient(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
