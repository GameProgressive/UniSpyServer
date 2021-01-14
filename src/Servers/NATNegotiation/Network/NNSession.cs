using System.Net;
using UniSpyLib.Network;
namespace NATNegotiation.Network
{
    public class NNSession : UDPSessionBase
    {
        public NNSession(UDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
