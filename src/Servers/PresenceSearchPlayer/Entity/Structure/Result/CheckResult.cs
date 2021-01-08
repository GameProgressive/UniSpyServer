using PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Result
{
    public class CheckResult : PSPResultBase
    {
        public uint ProfileID { get; set; }
        public CheckResult()
        {
        }

        public CheckResult(UniSpyRequestBase request) : base(request)
        {
        }
    }
}
