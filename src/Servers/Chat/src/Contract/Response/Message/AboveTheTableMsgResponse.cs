using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Message;
using UniSpy.Server.Chat.Contract.Result.Message;

namespace UniSpy.Server.Chat.Contract.Response.Message
{
    public sealed class AboveTheTableMsgResponse : ResponseBase
    {
        private new AboveTheTableMsgRequest _request => (AboveTheTableMsgRequest)base._request;
        private new AboveTheTableMsgResult _result => (AboveTheTableMsgResult)base._result;
        public AboveTheTableMsgResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

        public override void Build()
        {
            string cmdParams = $"{_result.TargetName} {_request.Message}";
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.AboveTheTableMsg, cmdParams);
        }
    }
}
