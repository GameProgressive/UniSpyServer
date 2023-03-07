using UniSpy.Server.Chat.Abstraction.BaseClass.Message;

namespace UniSpy.Server.Chat.Contract.Result.Message
{
    public sealed class PrivateResult : MessageResultBase
    {
        public bool IsBroadcastMessage { get; set; }
        public PrivateResult(){ }
    }
}