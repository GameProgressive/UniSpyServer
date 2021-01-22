using ServerBrowser.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace ServerBrowser.Entity.Structure.Response
{
    internal sealed class PushUpdateResponse : UpdateOptionResponseBase
    {
        public PushUpdateResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {

            base.BuildNormalResponse();
        }
    }
}
