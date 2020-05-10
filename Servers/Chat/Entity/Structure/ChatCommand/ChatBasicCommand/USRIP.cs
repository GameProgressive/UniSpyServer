using Chat.Entity.Structure.ChatResponse;

namespace Chat.Entity.Structure.ChatCommand
{
    public class USRIP : ChatCommandBase
    {
        public string BuildResponse(string ip)
        {
            return BuildReply(ChatReply.UserIP, "", $"@{ip}");
        }
    }
}
