using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Result.General;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.General
{
    public sealed class LoginResponse : ResponseBase
    {
        private new LoginResult _result => (LoginResult)base._result;
        public LoginResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(
                ResponseName.Login,
                cmdParams: $"* {_result.UserID} {_result.ProfileID}");
        }
    }
}
