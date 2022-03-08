using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class PSPDefaultResponse : ResponseBase
    {
        public PSPDefaultResponse(RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
        }
    }
}
