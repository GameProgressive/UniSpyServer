using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    public class GETCKEYReply
    {
        public static string BuildGetCKeyReply(string nickname, string channelName, string cookie, string flags)
        {
            return ChatResponseBase.BuildResponse(ChatReplyCode.GetCKey,
                $"* {channelName} {nickname} {cookie} {flags}");
        }

        public static string BuildEndOfGetCKeyReply(string channelName, string cookie)
        {
            return ChatResponseBase.BuildResponse(ChatReplyCode.EndGetCKey,
                  $"* {channelName} {cookie}",
                  "End Of /GETCKEY.");
        }
    }
}
