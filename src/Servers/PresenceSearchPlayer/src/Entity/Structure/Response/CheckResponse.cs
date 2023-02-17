using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class CheckResponse : ResponseBase
    {
        private new CheckResult _result => (CheckResult)base._result;

        public CheckResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            if (_result.ProfileId is null)
            {
                SendingBuffer = $@"\cur\1\final\";
            }
            else
            {
                SendingBuffer = $@"\cur\0\pid\{_result.ProfileId}\final\";
            }
        }
    }
}
