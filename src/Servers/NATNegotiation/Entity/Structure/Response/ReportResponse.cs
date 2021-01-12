using NATNegotiation.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Response
{
    internal sealed class ReportResponse : NNResponseBase
    {
        public ReportResponse(NNRequestBase request) : base(request)
        {
        }

        public ReportResponse(byte version, uint cookie) : base(version, cookie)
        {
        }
    }
}
