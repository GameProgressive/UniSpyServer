using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Result
{
    internal class AddressResult : InitResult
    {
        public AddressResult()
        {
            PacketType = NatPacketType.AddressReply;
        }
    }
}
