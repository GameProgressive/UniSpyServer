using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Request
{
    
    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}