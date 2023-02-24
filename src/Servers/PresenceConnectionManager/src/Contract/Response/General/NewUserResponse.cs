
using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;
using UniSpy.Server.PresenceSearchPlayer.Contract.Result;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class NewUserResponse : PresenceSearchPlayer.Contract.Response.NewUserResponse
    {
        public NewUserResponse(NewUserRequest request, NewUserResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\nur\\userid\{_result.User.UserId}\profileid\{_result.SubProfile.ProfileId}\id\{_request.OperationID}\final\";
        }
    }
}