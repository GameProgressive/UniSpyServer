using GameSpyLib.Network;
using System.Net;
namespace NatNegotiation.Server
{
    public class NatNegSession : TemplateUdpSession
    {
        public NatNegSession(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
