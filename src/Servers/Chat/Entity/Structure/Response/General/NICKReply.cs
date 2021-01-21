using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class NICKReply
    {
        public static string BuildWelcomeReply(ChatUserInfo userInfo)
        {
            return ChatResponseBase.BuildRPL(
                ChatReplyName.Welcome, userInfo.NickName, "Welcome to RetrosSpy!");
        }
    }
}
