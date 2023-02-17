using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Message
{
    
    public sealed class UnderTheTableMsgRequest : MsgRequestBase
    {
        public UnderTheTableMsgRequest(string rawRequest) : base(rawRequest){ }
    }
}
