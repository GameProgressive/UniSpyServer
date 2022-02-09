using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Message
{
    public sealed class NoticeResponse : ResponseBase
    {
        private new NoticeResult _result => (NoticeResult)base._result;
        private new NoticeRequest _request => (NoticeRequest)base._request;
        public NoticeResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, _result.TargetName, _request.Message);
        }
    }
}
