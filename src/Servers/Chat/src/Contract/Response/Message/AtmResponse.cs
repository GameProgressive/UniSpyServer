using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class AtmResponse : ResponseBase
    {
        private new MessageRequestBase _request => (MessageRequestBase)base._request;
        private new AtmResult _result => (AtmResult)base._result;
        public AtmResponse(RequestBase request, ResultBase result) : base(request, result){ }

        public override void Build()
        {
            string cmdParams = $"{_result.TargetName} {_request.Message}";
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.AboveTheTableMsg, cmdParams);
        }
    }
}
