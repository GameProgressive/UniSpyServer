using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Result
{
    public class GETCHANKEYResult : ChatResultBase
    {
        public ChatChannelUser ChannelUser { get; protected set; }
        public string ChannelName { get; protected set; }
        public string Cookie { get; protected set; }
        public string Flags { get; protected set; }

        public GETCHANKEYResult(ChatChannelUser channelUser, string channelName, string cookie, string flags)
        {
            ChannelUser = channelUser;
            ChannelName = channelName;
            Cookie = cookie;
            Flags = flags;
        }
    }
}
