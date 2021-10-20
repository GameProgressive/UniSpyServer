using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;

namespace UniSpyServer.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.KeepAlive)]
    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}