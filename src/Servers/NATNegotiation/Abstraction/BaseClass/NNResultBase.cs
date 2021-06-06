using NATNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Abstraction.BaseClass
{
    public class NNResultBase : UniSpyResultBase
    {
        public NatPacketType? PacketType { get; set; }
        public NNResultBase()
        {
        }
    }
}
