using CDKey.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace CDKey.Entity.Structure.Response
{
    internal sealed class CDKeyDefaultResponse : CDKeyResponseBase
    {
        public CDKeyDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
        }
    }
}