using System;
using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class ValidResult : PSPResultBase
    {
        public bool IsAccountValid;
        public ValidResult()
        {
        }

        public ValidResult(UniSpyRequestBase request) : base(request)
        {
        }
    }
}
