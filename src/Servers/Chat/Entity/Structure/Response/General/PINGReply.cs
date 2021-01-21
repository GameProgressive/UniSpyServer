using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class PINGReply
    {
        public static string BuildPingReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(ChatReplyName.PONG);
        }

    }
}
