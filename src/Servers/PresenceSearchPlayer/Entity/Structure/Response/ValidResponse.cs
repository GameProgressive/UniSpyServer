using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    internal sealed class ValidResponse : PSPResponseBase
    {
        private new ValidResult _result => (ValidResult)base._result;

        public ValidResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            if (_result.IsAccountValid)
            {
                SendingBuffer = @"\vr\1\final\";
            }
            else
            {
                SendingBuffer = @"\vr\0\final\";
            }
        }
    }
}
