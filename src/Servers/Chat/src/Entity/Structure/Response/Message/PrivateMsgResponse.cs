using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Message
{
    public sealed class PrivateMsgResponse : ResponseBase
    {
        private new PrivateMsgResult _result => (PrivateMsgResult)base._result;
        private new PrivateMsgRequest _request => (PrivateMsgRequest)base._request;
        public PrivateMsgResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix,
                                                      ResponseName.PrivateMsg,
                                                      _result.TargetName,
                                                      _request.Message);
        }
    }
}
