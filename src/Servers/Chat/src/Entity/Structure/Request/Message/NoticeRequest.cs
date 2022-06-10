using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Message
{
    
    public sealed class NoticeRequest : MsgRequestBase
    {
        public NoticeRequest(string rawRequest) : base(rawRequest){ }
    }
}
