using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
    public sealed class NamesRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public NamesRequest(string rawRequest) : base(rawRequest){ }
        public NamesRequest() { }
    }
}
