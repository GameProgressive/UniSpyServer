using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class KeepAliveResponse : ResponseBase
    {
        public KeepAliveResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\ka\\final\";
        }
    }
}
