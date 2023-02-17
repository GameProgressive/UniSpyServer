
using UniSpy.Server.PresenceSearchPlayer.Abstraction.BaseClass;

namespace UniSpy.Server.PresenceConnectionManager.Contract.Response
{
    public sealed class NewUserResponse : PresenceSearchPlayer.Contract.Response.NewUserResponse
    {
        public NewUserResponse(RequestBase request, ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\nur\\userid\{_result.User.UserId}\profileid\{_result.SubProfile.ProfileId}\id\{_request.OperationID}\final\";
        }
    }
}