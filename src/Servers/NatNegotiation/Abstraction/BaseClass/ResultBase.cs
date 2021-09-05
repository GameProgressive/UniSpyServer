using NatNegotiation.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Abstraction.BaseClass
{
    public class ResultBase : UniSpyResultBase
    {
        public NatPacketType? PacketType { get; set; }
        public ResultBase()
        {
        }
    }
}
