using NatNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Response
{
    internal sealed class ReportResponse : NNResponseBase
    {
        public ReportResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
