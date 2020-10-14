using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class PINGReply
    {
        public static string BuildPingReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(ChatReplyCode.PONG);
        }

    }
}
