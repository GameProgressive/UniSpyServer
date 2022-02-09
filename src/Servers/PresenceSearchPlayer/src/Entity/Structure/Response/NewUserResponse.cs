using UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Result;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure.Response
{
    public sealed class NewUserResponse : ResponseBase
    {
        public NewUserResponse(RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        private new NewUserResult _result => (NewUserResult)base._result;

        public override void Build()
        {
            SendingBuffer = $@"\nur\\pid\{_result.SubProfile.ProfileId}\final\";
        }
    }
}
