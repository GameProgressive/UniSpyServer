using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class PingRequest : RequestBase
    {
        public PingRequest(string rawRequest) : base(rawRequest){ }
    }
}
