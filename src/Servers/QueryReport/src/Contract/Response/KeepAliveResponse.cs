using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Contract.Request;

namespace UniSpy.Server.QueryReport.Contract.Response
{
    public sealed class KeepAliveResponse : ResponseBase
    {
        public KeepAliveResponse(KeepAliveRequest request) : base(request, null)
        {
        }
    }
}
