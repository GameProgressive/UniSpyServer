using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request
{

    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}