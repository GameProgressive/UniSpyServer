using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.Message
{
    public class PRIVMSGReply
    {
        public static string BuildPrivMsgReply(ChatUserInfo senderInfo, string targetName, string message)
        {
            return senderInfo.BuildReply(ChatReplyName.PRIVMSG, $"{targetName}", message);
        }
    }
}
