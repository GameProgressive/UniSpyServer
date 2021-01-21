using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class NAMEReply
    {
        public static string BuildNameReply(string nickName, string channelName, string nicks)
        {
            return ChatResponseBase.BuildRPL(
                    ChatReplyName.NameReply,
                    $"{nickName} = {channelName}", nicks);
        }
        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            return ChatResponseBase.BuildRPL(ChatReplyName.EndOfNames,
                    $"{nickName} {channelName}", @"End of /NAMES list.");
        }

    }
}
