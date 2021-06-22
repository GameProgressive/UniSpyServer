using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    internal class AddressResult : InitResult
    {
        public AddressResult()
        {
            PacketType = NatPacketType.AddressReply;
        }
    }
}
