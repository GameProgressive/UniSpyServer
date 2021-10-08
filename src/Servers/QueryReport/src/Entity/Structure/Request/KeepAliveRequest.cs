using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.contract;
using QueryReport.Entity.Enumerate;

namespace QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.KeepAlive)]
    internal class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}