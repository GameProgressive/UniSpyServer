using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Result
{
    internal sealed class ErtAckResult : InitResultBase
    {
        public ErtAckResult()
        {
            PacketType = NatPacketType.ErtAck;
        }
    }
}
