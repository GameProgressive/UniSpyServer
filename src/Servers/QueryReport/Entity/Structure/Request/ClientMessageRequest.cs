using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    public class ClientMessageRequest:QRRequestBase
    {
        public ClientMessageRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}
