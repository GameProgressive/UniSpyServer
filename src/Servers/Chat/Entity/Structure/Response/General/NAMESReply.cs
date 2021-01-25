using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class NAMEReply
    {
        public static string BuildNameReply(string nickName, string channelName, string nicks)
        {
            return ChatIRCReplyBuilder.Build(
                    ChatReplyName.NameReply,
                    $"{nickName} = {channelName}", nicks);
        }
        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            string cmdParams = $"{nickName} {channelName}";
            string tailing = @"End of /NAMES list.";
            return ChatIRCReplyBuilder.Build(
                ChatReplyName.EndOfNames,
                cmdParams,
                tailing);
        }

    }
}
