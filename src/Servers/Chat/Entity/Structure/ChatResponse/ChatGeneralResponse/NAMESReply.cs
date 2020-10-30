using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class NAMEReply
    {
        public static string BuildNameReply(string nickName, string channelName, string nicks)
        {
            return ChatReplyBase.BuildReply(
                    ChatReplyCode.NameReply,
                    $"{nickName} = {channelName}", nicks);
        }
        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.EndOfNames,
                    $"{nickName} {channelName}", @"End of /NAMES list.");
        }

    }
}
