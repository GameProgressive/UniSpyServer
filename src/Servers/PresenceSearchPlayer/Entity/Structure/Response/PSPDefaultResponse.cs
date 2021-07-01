using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal sealed class PSPDefaultResponse : PSPResponseBase
    {
        public PSPDefaultResponse(PSPRequestBase request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
