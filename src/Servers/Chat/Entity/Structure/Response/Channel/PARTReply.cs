using Chat.Entity.Structure.ChatChannel;

namespace Chat.Entity.Structure.Response.General
{
    public class PARTReply
    {
        public static string BuildPartReply(ChatChannelUser leaver, string channelName, string reason)
        {
            return leaver.BuildReply(ChatReplyCode.PART, channelName, reason);
        }
    }
}
