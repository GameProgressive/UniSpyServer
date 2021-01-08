using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class RegisterCDKeyResponse : PCMResponseBase
    {
        public RegisterCDKeyResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\rc\\final\";
        }
    }
}
