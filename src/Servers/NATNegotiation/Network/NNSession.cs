using System.Net;
using UniSpyLib.Network;
namespace NatNegotiation.Network
{
    internal sealed class NNSession : UniSpyUDPSessionBase
    {
        public NNSession(UniSpyUDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
