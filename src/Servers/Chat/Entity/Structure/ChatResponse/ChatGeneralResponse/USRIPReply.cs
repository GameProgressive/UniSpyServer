using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class USRIPReply
    {
        public static string BuildUserIPReply(string ip)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.UserIP, "", $"@{ip}");
        }
    }
}
