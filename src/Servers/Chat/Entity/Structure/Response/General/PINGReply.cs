using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.General
{
    public class PINGReply
    {
        public static string BuildPingReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(ChatReplyCode.PONG);
        }

    }
}
