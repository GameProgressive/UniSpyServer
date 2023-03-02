using UniSpy.Server.QueryReport.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.Contract.Request
{

    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}