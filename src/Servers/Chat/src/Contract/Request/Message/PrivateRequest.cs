using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Message
{
    
    public sealed class PrivateRequest : MessageRequestBase
    {
        public PrivateRequest(string rawRequest) : base(rawRequest){ }
    }
}
