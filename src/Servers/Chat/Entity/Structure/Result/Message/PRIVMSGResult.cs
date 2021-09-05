using Chat.Abstraction.BaseClass.Message;

namespace Chat.Entity.Structure.Result.Message
{
    internal sealed class PRIVMSGResult : ChatMsgResultBase
    {
        public bool IsBroadcastMessage { get; set; }
        public PRIVMSGResult()
        {
        }
    }
}