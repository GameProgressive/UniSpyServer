using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class NewProfileResponse : PCMResponseBase
    {
        public NewProfileResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = $@"\npr\\profileid\{SendingBuffer}\id\{_result.Request.OperationID}\final\";
        }
    }
}
