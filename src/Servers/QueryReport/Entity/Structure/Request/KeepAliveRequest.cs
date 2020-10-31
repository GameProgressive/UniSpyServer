using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    public class KeepAliveRequest : QRRequestBase
    {
        public KeepAliveRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
