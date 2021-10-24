using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Message
{
    public sealed class NoticeResponse : ResponseBase
    {
        private new NoticeResult _result => (NoticeResult)base._result;
        private new NoticeRequest _request => (NoticeRequest)base._request;
        public NoticeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, _result.TargetName, _request.Message);
        }
    }
}
