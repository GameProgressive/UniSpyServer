using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class RegisterNickResponse : PCMResponseBase
    {
        public RegisterNickResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = $@"\rn\\id\{_result.Request.OperationID}\final\";
        }
    }
}
