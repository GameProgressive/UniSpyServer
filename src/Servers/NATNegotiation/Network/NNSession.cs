using System.Net;
using UniSpyLib.Network;
namespace NATNegotiation.Network
{
    public class NNSession : UniSpyUDPSessionBase
    {
        public NNSession(UniSpyUDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
