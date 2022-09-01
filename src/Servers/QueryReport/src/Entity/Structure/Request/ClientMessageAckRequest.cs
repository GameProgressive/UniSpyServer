using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    
    public sealed class ClientMessageAckRequest : RequestBase
    {
        public ClientMessageAckRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}