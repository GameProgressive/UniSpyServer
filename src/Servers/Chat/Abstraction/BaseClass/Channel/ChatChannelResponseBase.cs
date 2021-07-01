using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Abstraction.BaseClass
{
    internal abstract class ChatChannelResponseBase : ChatResponseBase
    {
        protected new ChatChannelRequestBase _request => (ChatChannelRequestBase)base._request;
        protected ChatChannelResponseBase(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }
    }
}
