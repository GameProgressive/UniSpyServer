using System;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Response
{
    public class KeepAliveResponse : QRResponseBase
    {
        public KeepAliveResponse(QRRequestBase request) : base(request)
        {
        }

        public KeepAliveResponse(QRPacketType packetType, int instantKey) : base(packetType, instantKey)
        {
        }
    }
}
