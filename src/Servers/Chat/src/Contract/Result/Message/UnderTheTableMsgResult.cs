using UniSpy.Server.Chat.Abstraction.BaseClass.Message;

namespace UniSpy.Server.Chat.Contract.Result.Message
{
    public sealed class UnderTheTableMsgResult : MsgResultBase
    {
        public string Name { get; set; }
        public UnderTheTableMsgResult(){ }
    }
}