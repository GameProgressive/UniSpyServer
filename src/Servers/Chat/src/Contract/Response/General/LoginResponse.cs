using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class LoginResponse : ResponseBase
    {
        private new LoginResult _result => (LoginResult)base._result;
        public LoginResponse(RequestBase request, ResultBase result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = $":{ServerDomain} {ResponseName.Login} * {_result.UserID} {_result.ProfileId}\r\n";
        }
    }
}
