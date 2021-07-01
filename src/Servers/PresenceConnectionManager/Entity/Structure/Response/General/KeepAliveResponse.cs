using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class KeepAliveResponse : PCMResponseBase
    {
        public KeepAliveResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\ka\\final\";
        }
    }
}
