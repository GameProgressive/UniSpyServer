using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class NamesRequest : ChannelRequestBase
    {
        public new string ChannelName{ get => base.ChannelName; set => base.ChannelName = value; }
        public NamesRequest(string rawRequest) : base(rawRequest){ }
        public NamesRequest() { }
    }
}
