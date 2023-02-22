namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ChannelResponseBase : ResponseBase
    {
        protected new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        protected ChannelResponseBase(RequestBase request, ResultBase result) : base(request, result){ }
    }
}
