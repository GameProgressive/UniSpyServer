using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.Message
{
    public class ATMReply
    {
        public static string BuildATMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(ChatReplyCode.ATM, $"{name} {message}");
        }
    }
}
