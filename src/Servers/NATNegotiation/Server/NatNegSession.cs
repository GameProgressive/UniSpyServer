using UniSpyLib.Network;
using NATNegotiation.Entity.Structure;
using System.Net;
namespace NATNegotiation.Server
{
    public class NatNegSession : UDPSessionBase
    {
        public NNUserInfo UserInfo { get; protected set; }
        public NatNegSession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
            UserInfo = new NNUserInfo();
        }
    }
}
