using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    public class TOPICReply
    {
        public static string BuildNoTopicReply(string channelName)
        {
            return
                ChatResponseBase.BuildRPL(ChatReplyCode.NoTopic, channelName);
        }
        public static string BuildTopicReply(string channelName, string channelTopic)
        {
            return
                   ChatResponseBase.BuildRPL(ChatReplyCode.TOPIC, channelName, channelTopic);
        }
    }
}
