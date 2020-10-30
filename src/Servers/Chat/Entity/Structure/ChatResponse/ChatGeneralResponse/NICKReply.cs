using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class NICKReply
    {
        public static string BuildWelcomeReply(ChatUserInfo userInfo)
        {
            return ChatReplyBase.BuildReply(
                ChatReplyCode.Welcome, userInfo.NickName, "Welcome to RetrosSpy!");
        }
    }
}
