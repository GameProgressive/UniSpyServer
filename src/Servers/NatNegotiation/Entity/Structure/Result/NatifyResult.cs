using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Result
{
    internal sealed class NatifyResult : InitResultBase
    {
        public NatifyResult()
        {
            PacketType = NatPacketType.ErtTest;
        }
    }
}
