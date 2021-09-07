using QueryReport.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class QRDefaultResponse : ResponseBase
    {
        public QRDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
