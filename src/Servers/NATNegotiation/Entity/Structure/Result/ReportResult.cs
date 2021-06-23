using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;

namespace NatNegotiation.Entity.Structure.Result
{
    public class ReportResult : NNResultBase
    {
        public ReportResult()
        {
            PacketType = NatPacketType.ReportAck;
        }
    }
}
