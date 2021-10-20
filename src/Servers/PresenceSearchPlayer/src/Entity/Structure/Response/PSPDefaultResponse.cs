using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class PSPDefaultResponse : ResponseBase
    {
        public PSPDefaultResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
