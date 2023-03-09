using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class NoticeResponse : ResponseBase
    {
        private new NoticeResult _result => (NoticeResult)base._result;
        private new NoticeRequest _request => (NoticeRequest)base._request;
        public NoticeResponse(RequestBase request, ResultBase result) : base(request, result){ }
        public override void Build()
        {
            SendingBuffer = $":{_result.UserIRCPrefix} {ResponseName.Notice} {_result.TargetName} :{_request.Message}\r\n";
        }
    }
}
