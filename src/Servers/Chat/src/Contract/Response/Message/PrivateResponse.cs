using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class PrivateResponse : ResponseBase
    {
        private new PrivateResult _result => (PrivateResult)base._result;
        private new PrivateRequest _request => (PrivateRequest)base._request;
        public PrivateResponse(RequestBase request, ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix,
                ResponseName.PrivateMsg,
                _result.TargetName,
                _request.Message);
        }
    }
}
