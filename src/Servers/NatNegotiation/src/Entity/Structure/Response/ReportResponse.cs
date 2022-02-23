using UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Response
{
    public sealed class ReportResponse : ResponseBase
    {
        public ReportResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}
