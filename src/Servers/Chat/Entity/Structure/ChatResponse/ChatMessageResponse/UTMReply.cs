using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatMessageResponse
{
    public class UTMReply
    {
        public static string BuildUTMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(ChatReplyCode.UTM, name, message);
        }

    }
}
