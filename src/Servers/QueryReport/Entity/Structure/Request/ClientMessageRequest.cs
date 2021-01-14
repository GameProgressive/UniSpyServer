using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class ClientMessageRequest : QRRequestBase
    {
        public ClientMessageRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
