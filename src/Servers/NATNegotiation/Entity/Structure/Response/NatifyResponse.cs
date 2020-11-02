using System;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;

namespace NATNegotiation.Entity.Structure.Response
{
    public class NatifyResponse:InitResponse
    {
        public NatifyResponse(NatifyRequest request, EndPoint endPoint) : base(request, endPoint)
        {
            PacketType = NatPacketType.ErtTest;
        }
    }
}
