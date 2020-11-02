using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.General
{
    public class NICKReply
    {
        public static string BuildWelcomeReply(ChatUserInfo userInfo)
        {
            return ChatResponseBase.BuildResponse(
                ChatReplyCode.Welcome, userInfo.NickName, "Welcome to RetrosSpy!");
        }
    }
}
