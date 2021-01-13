using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    internal sealed class ErtAckResult : InitResult
    {
        public ErtAckResult()
        {
            PacketType = NatPacketType.ErtAck;
        }
    }
}
