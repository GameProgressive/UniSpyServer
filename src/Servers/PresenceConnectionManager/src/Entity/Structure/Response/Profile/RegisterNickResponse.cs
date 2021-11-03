using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class RegisterNickResponse : ResponseBase
    {
        public RegisterNickResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\rn\\id\{_request.OperationID}\final\";
        }
    }
}
