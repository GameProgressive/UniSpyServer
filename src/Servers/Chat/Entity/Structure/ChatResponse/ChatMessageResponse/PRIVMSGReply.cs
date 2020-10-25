using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatMessageResponse
{
    public class PRIVMSGReply
    {
        public static string BuildPrivMsgReply(ChatUserInfo senderInfo, string targetName, string message)
        {
            return senderInfo.BuildReply(ChatReplyCode.PRIVMSG, $"{targetName}", message);
        }
    }
}
