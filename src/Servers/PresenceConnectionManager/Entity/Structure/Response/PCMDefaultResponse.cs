using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class PCMDefaultResponse : PCMResponseBase
    {

        public PCMDefaultResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build() { }
    }
}
