using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class NewProfileResponse : ResponseBase
    {
        public NewProfileResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\npr\\profileid\{SendingBuffer}\id\{_request.OperationID}\final\";
        }
    }
}
