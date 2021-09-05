using NatNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Response
{
    internal sealed class NNDefaultResponse : ResponseBase
    {
        public NNDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
