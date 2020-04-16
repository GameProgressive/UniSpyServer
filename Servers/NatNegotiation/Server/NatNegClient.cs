using System;
using System.Net;
using GameSpyLib.Network;
namespace NatNegotiation.Server
{
    public class NatNegClient : TemplateUdpClient
    {
        public NatNegClient(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
