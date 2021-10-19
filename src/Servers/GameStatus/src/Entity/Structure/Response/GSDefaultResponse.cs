using GameStatus.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Response
{
    public sealed class GSDefaultResponse : ResponseBase
    {
        public GSDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
