using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Result.Channel
{
    public class JOINResult : ChatResultBase
    {
        public ChatChannelUser Joiner;
        public string ChannelName;

        public JOINResult(ChatChannelUser joiner, string channelName)
        {
            Joiner = joiner;
            ChannelName = channelName;
        }
    }
}
