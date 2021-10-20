using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Abstraction.BaseClass
{
    public abstract class ChannelResponseBase : ResponseBase
    {
        protected new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        protected ChannelResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}
