using GameSpyLib.Network;
using System.Net;
namespace NatNegotiation.Server
{
    public class NatNegClient : TemplateUdpClient
    {
        public NatNegClient(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
