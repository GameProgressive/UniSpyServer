using System;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    internal sealed class AddressResult : InitResult
    {
        public AddressResult()
        {
            PacketType = NatPacketType.AddressReply;
        }
    }
}
