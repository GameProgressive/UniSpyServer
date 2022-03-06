using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Message
{
    public sealed class PrivateMsgResponse : ResponseBase
    {
        private new PrivateMsgResult _result => (PrivateMsgResult)base._result;
        private new PrivateMsgRequest _request => (PrivateMsgRequest)base._request;
        public PrivateMsgResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix,
                ResponseName.PrivateMsg,
                _result.TargetName,
                _request.Message);
        }
    }
}
