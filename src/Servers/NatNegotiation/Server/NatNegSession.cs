using GameSpyLib.Network;
using NATNegotiation.Entity.Structure;
using System.Net;
namespace NATNegotiation.Server
{
    public class NatNegSession : TemplateUdpSession
    {
        public NNUserInfo UserInfo { get; protected set; }
        public NatNegSession(TemplateUdpServer server, EndPoint endPoint) : base(server, endPoint)
        {
            UserInfo = new NNUserInfo();
        }
    }
}
