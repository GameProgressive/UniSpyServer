using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;

namespace UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response
{
    public sealed class NewUserResponse : ResponseBase
    {
        private new NewUserResult _result => (NewUserResult)base._result;
        public NewUserResponse(RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\nur\\userid\{_result.User.UserId}\profileid\{_result.SubProfile.ProfileId}\id\{_request.OperationID}\final\";
        }
    }
}