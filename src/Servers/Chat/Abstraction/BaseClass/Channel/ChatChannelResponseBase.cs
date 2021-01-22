using Chat.Entity.Structure.Misc;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatChannelResponseBase : ChatResponseBase
    {
        protected new ChatChannelRequestBase _request => (ChatChannelRequestBase)base._request;
        protected ChatChannelResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildIRCErrorResponse()
        {
            SendingBuffer = ChatIRCReplyBuilder.BuildByIRCErrorCode(_result.IRCErrorCode, _request.ChannelName);
        }
    }
}
