using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;

namespace UniSpyServer.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.Echo)]
    public class EchoRequest : RequestBase
    {
        public EchoRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}