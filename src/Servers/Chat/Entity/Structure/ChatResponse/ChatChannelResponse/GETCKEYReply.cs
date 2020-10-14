namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class GETCKEYReply
    {
        public static string BuildGetCKeyReply(string nickname, string channelName, string cookie, string flags)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.GetCKey,
                $"* {channelName} {nickname} {cookie} {flags}");
        }

        public static string BuildEndOfGetCKeyReply(string channelName, string cookie)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.EndGetCKey,
                  $"* {channelName} {cookie}",
                  "End Of /GETCKEY.");
        }
    }
}
