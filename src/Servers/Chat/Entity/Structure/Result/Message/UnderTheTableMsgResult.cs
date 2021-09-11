using Chat.Abstraction.BaseClass.Message;

namespace Chat.Entity.Structure.Result.Message
{
    internal sealed class UnderTheTableMsgResult : MsgResultBase
    {
        public string Name { get; set; }
        public UnderTheTableMsgResult()
        {
        }
    }
}