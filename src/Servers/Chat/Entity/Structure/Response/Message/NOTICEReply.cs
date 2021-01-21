using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.Message
{
    public class NOTICEReply
    {
        public static string BuildNoticeReply(ChatUserInfo senderInfo, string targetName, string message)
        {
            return senderInfo.BuildReply(ChatReplyName.NOTICE, $"{targetName}", message);
        }

    }
}
