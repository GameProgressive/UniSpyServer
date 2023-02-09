using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request
{

    public sealed class ClientMessageAckRequest : RequestBase
    {
        public ClientMessageAckRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}