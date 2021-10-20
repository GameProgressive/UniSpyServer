using UniSpyServer.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Result.Channel
{
    public sealed class SetCKeyResult : ResultBase
    {
        public bool IsSetOthersKeyValue { get; set; }
        public string NickName { get; set; }
        public string ChannelName { get; set; }

        public SetCKeyResult()
        {
            IsSetOthersKeyValue = false;
        }
    }
}
