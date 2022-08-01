using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Message
{
    
    public sealed class PrivateMsgRequest : MsgRequestBase
    {
        public PrivateMsgRequest(string rawRequest) : base(rawRequest){ }
    }
}
