using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Result
{
    internal sealed class AddressResult : InitResultBase
    {
        public AddressResult()
        {
            PacketType = NatPacketType.AddressReply;
        }
    }
}
