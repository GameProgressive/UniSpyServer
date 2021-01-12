using System;
using System.Net;
using NATNegotiation.Entity.Enumerate;
using NATNegotiation.Entity.Structure.Request;

namespace NATNegotiation.Entity.Structure.Response
{
    internal sealed class AddressResponse : InitResponse
    {
        public AddressResponse(AddressRequest request, EndPoint endPoint) : base(request, endPoint)
        {
            PacketType = NatPacketType.AddressReply;
        }
    }
}
