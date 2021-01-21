using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.Message
{
    public class UTMReply
    {
        public static string BuildUTMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(ChatReplyName.UTM, name, message);
        }

    }
}
