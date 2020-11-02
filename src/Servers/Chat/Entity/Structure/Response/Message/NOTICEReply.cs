using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.Message
{
    public class NOTICEReply
    {
        public static string BuildNoticeReply(ChatUserInfo senderInfo, string targetName, string message)
        {
            return senderInfo.BuildReply(ChatReplyCode.NOTICE, $"{targetName}", message);
        }

    }
}
