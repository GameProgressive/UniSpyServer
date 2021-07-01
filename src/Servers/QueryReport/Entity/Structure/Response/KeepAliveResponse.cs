using QueryReport.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class KeepAliveResponse : QRResponseBase
    {
        public KeepAliveResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
