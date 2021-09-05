using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class NewProfileResponse : PCMResponseBase
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
