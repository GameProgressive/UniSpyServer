using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal class PSPBasicResponse : PSPResponseBase
    {
        public PSPBasicResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
        }
    }
}
