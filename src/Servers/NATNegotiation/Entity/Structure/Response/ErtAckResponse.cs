
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;

namespace NATNegotiation.Entity.Structure.Response
{
    internal sealed class ErtAckResponse : InitResponse
    {
        public ErtAckResponse(ErtAckRequest request, EndPoint endPoint) : base(request, endPoint)
        {
            PacketType = NatPacketType.ErtAck;
        }
    }
}
