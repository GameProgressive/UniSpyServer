using GameSpyLib.Network;
using NatNegotiation.Entity.Structure;
using System.Net;
namespace NatNegotiation.Server
{
    public class NatNegSession : TemplateUdpSession
    {
        public NatNegUserInfo UserInfo { get; protected set; }
        public NatNegSession(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
            UserInfo = new NatNegUserInfo();
        }
    }
}
