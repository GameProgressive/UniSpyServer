using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Request
{
    public class EchoRequest : QRRequestBase
    {
        public EchoRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public EchoRequest(QRPacketType packetType, int instantKey) : base(packetType, instantKey)
        {
        }
    }
}
