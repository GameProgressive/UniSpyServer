using PresenceSearchPlayer.Abstraction.BaseClass;
using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Entity.Structure.Response
{
    public class NewUserResponse : PSPResponseBase
    {
        public NewUserResponse(PSPRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected new NewUserResult _result => (NewUserResult)base._result;
        protected override void BuildErrorResponse()
        {
            if (_result.ErrorCode >= GPErrorCode.NewUser && _result.ErrorCode <= GPErrorCode.NewUserUniquenickInvalid)
            {
                SendingBuffer = $@"\nur\{_result.ErrorCode}\final\";
            }
            else
            {
                base.BuildErrorResponse();
            }
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = $@"\nur\\pid\{_result.SubProfile.Profileid}\final\";
        }
    }
}
