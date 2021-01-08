using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class PSPBasicResult : PSPResultBase
    {
        public PSPBasicResult()
        {
        }

        public PSPBasicResult(UniSpyRequestBase request) : base(request)
        {
        }
    }
}
