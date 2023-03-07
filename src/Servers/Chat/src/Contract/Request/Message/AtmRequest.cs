using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Message
{
    
    public sealed class AtmRequest : MessageRequestBase
    {
        public AtmRequest(string rawRequest) : base(rawRequest){ }
    }
}
