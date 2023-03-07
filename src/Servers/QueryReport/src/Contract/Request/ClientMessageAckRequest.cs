using UniSpy.Server.QueryReport.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Contract.Request
{

    public sealed class ClientMessageAckRequest : RequestBase
    {
        public ClientMessageAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}