using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Message
{
    
    public sealed class NoticeRequest : MessageRequestBase
    {
        public NoticeRequest(string rawRequest) : base(rawRequest){ }
    }
}
