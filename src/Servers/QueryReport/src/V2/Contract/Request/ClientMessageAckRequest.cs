using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V2.Contract.Request
{

    public sealed class ClientMessageAckRequest : RequestBase
    {
        public ClientMessageAckRequest(byte[] rawRequest) : base(rawRequest)
        {
        }
    }
}