using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Contract.Response
{
    public sealed class KeepAliveResponse : ResponseBase
    {
        public KeepAliveResponse(KeepAliveRequest request) : base(request, null)
        {
        }
    }
}
