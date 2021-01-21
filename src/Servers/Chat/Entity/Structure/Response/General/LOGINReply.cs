using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class LOGINReply
    {
        public static string BuildLoginReply(uint userid, uint profileid)
        {
            return ChatResponseBase.BuildRPL(ChatReplyName.Login, $"param1 {userid} {profileid}");
        }
    }
}
