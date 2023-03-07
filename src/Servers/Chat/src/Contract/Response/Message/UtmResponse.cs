using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class UtmResponse : ResponseBase
    {
        private new UtmResult _result => (UtmResult)base._result;
        private new UtmRequest _request => (UtmRequest)base._request;
        public UtmResponse(RequestBase request, ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.UnderTheTableMsg, _result.TargetName, _request.Message);
        }
    }
}
