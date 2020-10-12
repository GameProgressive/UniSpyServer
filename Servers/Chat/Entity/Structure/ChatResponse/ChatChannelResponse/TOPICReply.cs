using System;
namespace Chat.Entity.Structure.ChatResponse.ChatChannelResponse
{
    public class TOPICReply
    {
        public static string BuildNoTopicReply(string channelName)
        {
            return
                ChatReplyBase.BuildReply(ChatReplyCode.NoTopic, channelName);
        }
        public static string BuildTopicReply(string channelName, string channelTopic)
        {
            return
                   ChatReplyBase.BuildReply(ChatReplyCode.TOPIC, channelName, channelTopic);
        }
    }
}
