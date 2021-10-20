using UniSpyServer.NatNegotiation.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Response
{
    public sealed class ReportResponse : ResponseBase
    {
        public ReportResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
