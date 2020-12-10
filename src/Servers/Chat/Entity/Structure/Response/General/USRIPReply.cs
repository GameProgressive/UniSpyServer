using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    public class USRIPReply
    {
        public static string BuildUserIPReply(string ip)
        {
            return ChatResponseBase.BuildRPL(ChatReplyCode.UserIP, "", $"@{ip}");
        }
    }
}
