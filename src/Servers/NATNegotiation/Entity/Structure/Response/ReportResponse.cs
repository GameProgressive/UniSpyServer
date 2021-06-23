using NatNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Response
{
    internal sealed class ReportResponse : NNResponseBase
    {
        public ReportResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
