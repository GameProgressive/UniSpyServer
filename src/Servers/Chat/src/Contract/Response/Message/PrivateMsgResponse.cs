using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class PrivateMsgResponse : ResponseBase
    {
        private new PrivateMsgResult _result => (PrivateMsgResult)base._result;
        private new PrivateMsgRequest _request => (PrivateMsgRequest)base._request;
        public PrivateMsgResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix,
                ResponseName.PrivateMsg,
                _result.TargetName,
                _request.Message);
        }
    }
}
