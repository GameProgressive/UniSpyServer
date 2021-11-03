using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class RegisterCDKeyResponse : ResponseBase
    {
        public RegisterCDKeyResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = @"\rc\\final\";
        }
    }
}
