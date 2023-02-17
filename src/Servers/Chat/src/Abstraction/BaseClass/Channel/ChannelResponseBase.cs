namespace UniSpy.Server.Chat.Abstraction.BaseClass
{
    public abstract class ChannelResponseBase : ResponseBase
    {
        protected new ChannelRequestBase _request => (ChannelRequestBase)base._request;
        protected ChannelResponseBase(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
    }
}
