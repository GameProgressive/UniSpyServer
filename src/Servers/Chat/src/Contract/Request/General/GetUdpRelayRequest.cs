using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class GetUdpRelayRequest : ChannelRequestBase
    {
        public GetUdpRelayRequest(string rawRequest) : base(rawRequest){ }
    }
}
