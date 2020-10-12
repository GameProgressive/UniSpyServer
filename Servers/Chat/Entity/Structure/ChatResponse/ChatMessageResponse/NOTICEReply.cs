using System;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatMessageResponse
{
    public class NOTICEReply
    {
        public static string BuildNoticeReply(ChatUserInfo senderInfo, string targetName, string message)
        {
            return senderInfo.BuildReply(ChatReplyCode.NOTICE, $"{targetName}", message);
        }

    }
}
