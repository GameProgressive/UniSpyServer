using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class NAMESResult : ChatResultBase
    {
        public string AllChannelUserNick { get; set; }
        public string ChannelName { get; set; }
        public string RequesterNickName { get; set; }
        public NAMESResult()
        {
        }
    }
}
