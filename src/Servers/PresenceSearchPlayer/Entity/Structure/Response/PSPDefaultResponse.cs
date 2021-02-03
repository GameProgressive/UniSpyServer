using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal class PSPDefaultResponse : PSPResponseBase
    {
        public PSPDefaultResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
        }
    }
}
