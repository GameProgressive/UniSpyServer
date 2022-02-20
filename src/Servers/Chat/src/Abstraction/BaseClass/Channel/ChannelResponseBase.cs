namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public abstract class ChannelResponseBase : ResponseBase
    {
        protected new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        protected ChannelResponseBase(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }
    }
}
