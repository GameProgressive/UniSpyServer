using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
    public sealed class GetUdpRelayRequest : ChannelRequestBase
    {
        public GetUdpRelayRequest(string rawRequest) : base(rawRequest){ }
    }
}
