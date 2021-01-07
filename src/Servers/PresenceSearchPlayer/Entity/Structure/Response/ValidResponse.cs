using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class ValidResponse : PSPResponseBase
    {
        protected new ValidResult _result => (ValidResult)base._result;
        public ValidResponse(PSPResultBase result) : base(result)
        {
        }

        protected override void BuildNormalResponse()
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
