using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    public class NatifyResult : NNResultBase
    {
        public NatifyResult()
        {
            PacketType = NatPacketType.ErtTest;
        }
    }
}
