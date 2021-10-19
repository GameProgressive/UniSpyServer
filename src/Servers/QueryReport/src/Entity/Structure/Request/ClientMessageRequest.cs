using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.contract;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.ClientMessage)]
    public sealed class ClientMessageRequest : RequestBase
    {
        public new uint InstantKey
        {
            get => base.InstantKey;
            set => base.InstantKey = value;
        }
        public ClientMessageRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public ClientMessageRequest()
        {
        }
    }
}
