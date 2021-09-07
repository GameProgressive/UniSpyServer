using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    internal sealed class PRIVMSGResponse : ResponseBase
    {
        private new PRIVMSGResult _result => (PRIVMSGResult)base._result;
        private new PrivateMsgRequest _request => (PrivateMsgRequest)base._request;
        public PRIVMSGResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.UserIRCPrefix,
                                                      ChatReplyName.PRIVMSG,
                                                      _result.TargetName,
                                                      _request.Message);
        }
    }
}
