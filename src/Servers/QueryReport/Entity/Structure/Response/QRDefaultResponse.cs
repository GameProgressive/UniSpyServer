using QueryReport.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Response
{
    internal sealed class QRDefaultResponse : QRResponseBase
    {
        public QRDefaultResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
