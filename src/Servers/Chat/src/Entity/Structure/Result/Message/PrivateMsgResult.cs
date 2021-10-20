using UniSpyServer.Chat.Abstraction.BaseClass.Message;

namespace UniSpyServer.Chat.Entity.Structure.Result.Message
{
    public sealed class PrivateMsgResult : MsgResultBase
    {
        public bool IsBroadcastMessage { get; set; }
        public PrivateMsgResult()
        {
        }
    }
}