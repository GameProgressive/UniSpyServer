using Chat.Abstraction.BaseClass.Message;

namespace Chat.Entity.Structure.Result.Message
{
    public sealed class PrivateMsgResult : MsgResultBase
    {
        public bool IsBroadcastMessage { get; set; }
        public PrivateMsgResult()
        {
        }
    }
}