using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class PSPBasicResponse : PSPResponseBase
    {
        public PSPBasicResponse(UniSpyResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
        {
        }
    }
}
