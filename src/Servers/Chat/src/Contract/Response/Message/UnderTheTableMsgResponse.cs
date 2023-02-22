using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class UnderTheTableMsgResponse : ResponseBase
    {
        private new UnderTheTableMsgResult _result => (UnderTheTableMsgResult)base._result;
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        public UnderTheTableMsgResponse(RequestBase request, ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.UnderTheTableMsg, _result.Name, _request.Message);
        }
    }
}
