using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class SETCKEYResult : ChatResultBase
    {
        public bool IsSetOthersKeyValue { get; set; }
        public string NickName { get; set; }
        public string ChannelName { get; set; }

        public SETCKEYResult()
        {
            IsSetOthersKeyValue = false;
        }
    }
}
