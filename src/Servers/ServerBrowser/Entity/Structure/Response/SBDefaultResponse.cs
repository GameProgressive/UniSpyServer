using ServerBrowser.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Response
{
    internal sealed class SBDefaultResponse : SBResponseBase
    {
        public SBDefaultResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
