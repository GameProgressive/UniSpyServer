using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    internal sealed class UnderTheTableMsgResponse : ResponseBase
    {
        private new UnderTheTableMsgResult _result => (UnderTheTableMsgResult)base._result;
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        public UnderTheTableMsgResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.UserIRCPrefix, ChatReplyName.UTM, _result.Name, _request.Message);
        }
    }
}
