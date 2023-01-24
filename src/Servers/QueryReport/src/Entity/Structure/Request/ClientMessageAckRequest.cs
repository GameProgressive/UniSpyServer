using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request
{
    
    public sealed class ClientMessageAckRequest : RequestBase
    {
        public ClientMessageAckRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}