using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Message
{
    
    public sealed class UtmRequest : MessageRequestBase
    {
        public UtmRequest(string rawRequest) : base(rawRequest){ }
    }
}
