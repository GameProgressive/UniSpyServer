using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    public class LOGINReply
    {
        public static string BuildLoginReply(uint userid, uint profileid)
        {
            return ChatResponseBase.BuildRPL(ChatReplyCode.Login, $"param1 {userid} {profileid}");
        }
    }
}
