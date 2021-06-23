using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Abstraction.BaseClass
{
    public class NNResultBase : UniSpyResultBase
    {
        public NatPacketType? PacketType { get; set; }
        public NNResultBase()
        {
        }
    }
}
