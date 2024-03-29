using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class UserIPResponse : ResponseBase
    {
        private new UserIPResult _result => (UserIPResult)base._result;
        public UserIPResponse(RequestBase request, ResultBase result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = $":{ServerDomain} {ResponseName.UserIP} :@{_result.RemoteIPAddress}\r\n";
        }
    }
}
