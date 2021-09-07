using QueryReport.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class KeepAliveResponse : ResponseBase
    {
        public KeepAliveResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
