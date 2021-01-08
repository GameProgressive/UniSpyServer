using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class KeepAliveResponse : PCMResponseBase
    {
        public KeepAliveResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = @"\ka\\final\";
        }
    }
}
