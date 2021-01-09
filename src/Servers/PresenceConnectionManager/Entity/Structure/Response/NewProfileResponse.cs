using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class NewProfileResponse : PCMResponseBase
    {
        public NewProfileResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = $@"\npr\\profileid\{SendingBuffer}\id\{_request.OperationID}\final\";
        }
    }
}
