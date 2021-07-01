using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Abstraction.BaseClass
{
    public class NNResultBase : UniSpyResult
    {
        public NatPacketType? PacketType { get; set; }
        public NNResultBase()
        {
        }
    }
}
