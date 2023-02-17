using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class ValidResponse : ResponseBase
    {
        private new ValidResult _result => (ValidResult)base._result;

        public ValidResponse(RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result)
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
