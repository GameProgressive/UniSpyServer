using UniSpyLib.Network;
using System.Net;

namespace CDKey.Server
{
    public class CDKeySession : TemplateUdpSession
    {
        public CDKeySession(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
