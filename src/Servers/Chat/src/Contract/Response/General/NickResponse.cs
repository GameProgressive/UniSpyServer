using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class NickResponse : ResponseBase
    {
        private new NickRequest _request => (NickRequest)base._request;
        public NickResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
        public override void Build()
        {
            SendingBuffer = BuildWelcomeReply(_request.NickName);
        }
        public static string BuildWelcomeReply(string nickName)
        {
            return IRCReplyBuilder.Build(
                ResponseName.Welcome,
                cmdParams: nickName,
                tailing: "Welcome to UniSpy!");
        }
    }
}
