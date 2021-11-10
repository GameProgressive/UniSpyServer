using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel
{
    public sealed class NamesResult : ResultBase
    {
        public string AllChannelUserNick { get; set; }
        public string ChannelName { get; set; }
        public string RequesterNickName { get; set; }
        public NamesResult()
        {
        }
    }
}
