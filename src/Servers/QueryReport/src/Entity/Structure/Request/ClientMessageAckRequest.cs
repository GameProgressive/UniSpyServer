using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.ClientMessageAck)]
    public sealed class ClientMessageAckRequest : RequestBase
    {
        public ClientMessageAckRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}