using UniSpy.Server.Chat.Abstraction.BaseClass.Message;

namespace UniSpy.Server.Chat.Contract.Result.Message
{
    public sealed class PrivateMsgResult : MsgResultBase
    {
        public bool IsBroadcastMessage { get; set; }
        public PrivateMsgResult(){ }
    }
}