using System;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class WHOReply
    {
        public static string BuildWhoReply(string channelName, ChatUserInfo userInfo, string modes)
        {
            return ChatReplyBase.BuildReply(
                    ChatReplyCode.WhoReply,
                    $"param1 {channelName} " +
                    $"{userInfo.UserName} {userInfo.PublicIPAddress} param5 {userInfo.NickName} {modes} param8");
        }

        public static string BuildEndOfWhoReply(string name)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.EndOfWho, $"param1 {name} param3", "End of WHO.");
        }
    }
}
