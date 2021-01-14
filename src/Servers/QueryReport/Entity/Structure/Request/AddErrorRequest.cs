using QueryReport.Abstraction.BaseClass;

namespace QueryReport.Entity.Structure.Request
{
    internal sealed class AddErrorRequest : QRRequestBase
    {
        public AddErrorRequest(object rawRequest) : base(rawRequest)
        {
        }
    }
}
