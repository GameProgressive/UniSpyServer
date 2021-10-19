using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class CheckResponse : ResponseBase
    {
        private new CheckResult _result => (CheckResult)base._result;

        public CheckResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\cur\0\pid\{_result.ProfileID}\final\";
        }
    }
}
