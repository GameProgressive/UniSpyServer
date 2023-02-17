using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.Message
{
    
    public sealed class AboveTheTableMsgRequest : MsgRequestBase
    {
        public AboveTheTableMsgRequest(string rawRequest) : base(rawRequest){ }
    }
}
