using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public sealed class ValidResponse : ResponseBase
    {
        private new ValidResult _result => (ValidResult)base._result;

        public ValidResponse(RequestBase request, ResultBase result) : base(request, result)
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
