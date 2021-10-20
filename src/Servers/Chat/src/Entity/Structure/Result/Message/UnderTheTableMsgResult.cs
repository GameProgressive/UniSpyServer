using UniSpyServer.Chat.Abstraction.BaseClass.Message;

namespace UniSpyServer.Chat.Entity.Structure.Result.Message
{
    public sealed class UnderTheTableMsgResult : MsgResultBase
    {
        public string Name { get; set; }
        public UnderTheTableMsgResult()
        {
        }
    }
}