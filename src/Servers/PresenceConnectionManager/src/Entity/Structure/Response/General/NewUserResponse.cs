using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class NewUserResponse : ResponseBase
    {
        private new NewUserResult _result => (NewUserResult)base._result;
        public NewUserResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = $@"\nur\\userid\{_result.User.Userid}\profileid\{_result.SubProfile.Profileid}\id\{_request.OperationID}\final\";
        }
    }
}