using UniSpyServer.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.PresenceSearchPlayer.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class NewUserResponse : ResponseBase
    {
        public NewUserResponse(RequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new NewUserResult _result => (NewUserResult)base._result;

        public override void Build()
        {
            SendingBuffer = $@"\nur\\pid\{_result.SubProfile.Profileid}\final\";
        }
    }
}
