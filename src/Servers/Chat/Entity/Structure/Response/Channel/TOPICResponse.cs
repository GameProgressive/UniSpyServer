using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.Channel
{
    public class TOPICResponse
    {
        public static string BuildNoTopicReply(string channelName)
        {
            return
                ChatResponseBase.BuildRPL(ChatReplyName.NoTopic, channelName);
        }
        public static string BuildTopicReply(string channelName, string channelTopic)
        {
            return
                   ChatResponseBase.BuildRPL(ChatReplyName.TOPIC, channelName, channelTopic);
        }
    }
}
