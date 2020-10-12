using System;
using Chat.Entity.Structure.ChatChannel;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class JOINReply
    {
        public static string BuildJoinReply(ChatChannelUser joiner, string channelName)
        {
            return joiner.BuildReply(ChatReplyCode.JOIN, channelName);
        }
    }
}
