using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;

namespace UniSpyServer.QueryReport.Entity.Structure.Request
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
