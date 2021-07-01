using NatNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NatNegotiation.Entity.Structure.Response
{
    internal sealed class NNDefaultResponse : NNResponseBase
    {
        public NNDefaultResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
