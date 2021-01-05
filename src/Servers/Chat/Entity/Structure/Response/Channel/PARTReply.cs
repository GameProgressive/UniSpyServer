using Chat.Entity.Structure.ChannelInfo;

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
