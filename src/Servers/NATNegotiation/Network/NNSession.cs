using System.Net;
using UniSpyLib.Network;
namespace NATNegotiation.Network
{
    internal sealed class NNSession : UniSpyUDPSessionBase
    {
        public NNSession(UniSpyUDPServerBase server, EndPoint endPoint) : base(server, endPoint)
        {
        }
    }
}
