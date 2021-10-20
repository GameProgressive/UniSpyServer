using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.NatNegotiation.Entity.Enumerate;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Result
{
    public sealed class ReportResult : ResultBase
    {
        public ReportResult()
        {
            PacketType = NatPacketType.ReportAck;
        }
    }
}
