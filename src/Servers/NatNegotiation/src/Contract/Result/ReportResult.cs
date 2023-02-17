using UniSpy.Server.NatNegotiation.Abstraction.BaseClass;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Contract.Result
{
    public sealed class ReportResult : ResultBase
    {
        public ReportResult()
        {
            PacketType = ResponseType.ReportAck;
        }
    }
}
