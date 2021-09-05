using PresenceSearchPlayer.Abstraction.BaseClass;
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

        public override void Build()
        {
            SendingBuffer = $@"\nur\\pid\{_result.SubProfile.Profileid}\final\";
        }
    }
}
