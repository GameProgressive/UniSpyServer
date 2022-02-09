using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Message
{
    public sealed class UnderTheTableMsgResponse : ResponseBase
    {
        private new UnderTheTableMsgResult _result => (UnderTheTableMsgResult)base._result;
        private new UnderTheTableMsgRequest _request => (UnderTheTableMsgRequest)base._request;
        public UnderTheTableMsgResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.UnderTheTableMsg, _result.Name, _request.Message);
        }
    }
}
