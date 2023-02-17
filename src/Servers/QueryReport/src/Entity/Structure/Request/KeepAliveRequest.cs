using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;

namespace UniSpy.Server.QueryReport.V2.Entity.Structure.Request
{

    public class KeepAliveRequest : RequestBase
    {
        public KeepAliveRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}