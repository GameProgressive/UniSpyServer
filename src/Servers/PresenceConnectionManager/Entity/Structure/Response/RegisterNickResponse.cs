using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class RegisterNickResponse : PCMResponseBase
    {
        public RegisterNickResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = $@"\rn\\id\{_request.OperationID}\final\";
        }
    }
}
