using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Result
{
    public sealed class ReportResult : ResultBase
    {
        public ReportResult()
        {
            PacketType = NatPacketType.ReportAck;
        }
    }
}
