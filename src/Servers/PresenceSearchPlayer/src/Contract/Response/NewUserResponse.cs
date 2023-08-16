using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceSearchPlayer.Contract.Response
{
    public class NewUserResponse : ResponseBase
    {
        protected new NewUserRequest _request => (NewUserRequest)base._request;
        protected new NewUserResult _result => (NewUserResult)base._result;
        public NewUserResponse(NewUserRequest request, NewUserResult result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = $@"\nur\\pid\{_result.SubProfile.Profileid}\final\";
        }
    }
}
