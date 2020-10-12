using System;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatMessageResponse
{
    public class ATMReply
    {
        public static string BuildATMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(ChatReplyCode.ATM, $"{name} {message}");
        }
    }
}
