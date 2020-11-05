using UniSpyLib.Network;
using NATNegotiation.Entity.Structure;
using System.Net;
namespace NATNegotiation.Network
{
    public class NNSession : UDPSessionBase
    {
        public NNUserInfo UserInfo { get; protected set; }
        public NNSession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
            UserInfo = new NNUserInfo();
        }
    }
}
