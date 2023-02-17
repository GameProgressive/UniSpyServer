using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public class NewUserResponse : ResponseBase
    {
        protected new NewUserResult _result => (NewUserResult)base._result;

        public NewUserResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = $@"\nur\\pid\{_result.SubProfile.ProfileId}\final\";
        }
    }
}
