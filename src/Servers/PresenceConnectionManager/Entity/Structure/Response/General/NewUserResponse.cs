using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Entity.Structure.Response
{
    internal sealed class NewUserResponse : PSPResponseBase
    {
        private new NewUserResult _result => (NewUserResult)base._result;
        public NewUserResponse(PSPRequestBase request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer =
                $@"\nur\\userid\{_result.User.Userid}\profileid\{_result.SubProfile.Profileid}\id\{_request.OperationID}\final\";
        }
    }
}
