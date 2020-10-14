namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class LOGINReply
    {
        public static string BuildLoginReply(uint userid, uint profileid)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.Login, $"param1 {userid} {profileid}");
        }
    }
}
