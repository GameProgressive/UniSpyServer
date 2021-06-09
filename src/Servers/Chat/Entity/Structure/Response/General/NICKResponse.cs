using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.General;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class NICKResponse : ChatResponseBase
    {
        private new NICKRequest _request => (NICKRequest)base._request;
        public NICKResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = BuildWelcomeReply(_request.NickName);
        }
        public static string BuildWelcomeReply(string nickName)
        {
            return ChatIRCReplyBuilder.Build(
                ChatReplyName.Welcome,
                cmdParams: nickName,
                tailing: "Welcome to RetrosSpy!");
        }


    }
}
