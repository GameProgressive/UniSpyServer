using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    public class PCMBasicResponse : PCMResponseBase
    {
        public PCMBasicResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse() { }
    }
}
