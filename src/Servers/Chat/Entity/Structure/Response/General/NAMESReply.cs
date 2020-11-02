using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    public class NAMEReply
    {
        public static string BuildNameReply(string nickName, string channelName, string nicks)
        {
            return ChatResponseBase.BuildResponse(
                    ChatReplyCode.NameReply,
                    $"{nickName} = {channelName}", nicks);
        }
        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            return ChatResponseBase.BuildResponse(ChatReplyCode.EndOfNames,
                    $"{nickName} {channelName}", @"End of /NAMES list.");
        }

    }
}
