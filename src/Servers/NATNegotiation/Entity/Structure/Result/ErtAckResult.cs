using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    public class ErtAckResult : InitResult
    {
        public ErtAckResult()
        {
            PacketType = NatPacketType.ErtAck;
        }
    }
}
