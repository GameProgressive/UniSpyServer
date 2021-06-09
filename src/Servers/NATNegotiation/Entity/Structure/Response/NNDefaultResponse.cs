using NATNegotiation.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace NATNegotiation.Entity.Structure.Response
{
    internal sealed class NNDefaultResponse : NNResponseBase
    {
        public NNDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
