using System;
using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;

namespace NATNegotiation.Entity.Structure.Result
{
    public class ReportResult:NNResultBase
    {
        public ReportResult()
        {
            PacketType = NatPacketType.ReportAck;
        }
    }
}
