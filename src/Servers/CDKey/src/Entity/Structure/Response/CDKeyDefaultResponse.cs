using CDKey.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Entity.Structure.Response
{
    internal sealed class CDKeyDefaultResponse : ResponseBase
    {
        public CDKeyDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            throw new System.NotImplementedException();
        }
    }
}