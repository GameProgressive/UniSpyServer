using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal class CheckResponse : PSPResponseBase
    {
        protected new CheckResult _result => (CheckResult)base._result;

        public CheckResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\cur\0\pid\{_result.ProfileID}\final\";
        }
    }
}
