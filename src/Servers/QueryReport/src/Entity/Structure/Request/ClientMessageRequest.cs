using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.ClientMessage)]
    public sealed class ClientMessageRequest : RequestBase
    {
        public new uint? InstantKey { get => base.InstantKey; set => base.InstantKey = value; }
        public ClientMessageRequest(byte[] rawRequest) : base(rawRequest)
        {
        }

        public ClientMessageRequest()
        {
        }
    }
}
