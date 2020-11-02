using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.General
{
    public class WHOReply
    {
        public static string BuildWhoReply(string channelName, ChatUserInfo userInfo, string modes)
        {
            return ChatResponseBase.BuildResponse(
                    ChatReplyCode.WhoReply,
                    $"param1 {channelName} " +
                    $"{userInfo.UserName} {userInfo.PublicIPAddress} param5 {userInfo.NickName} {modes} param8");
        }

        public static string BuildEndOfWhoReply(string name)
        {
            return ChatResponseBase.BuildResponse(ChatReplyCode.EndOfWho, $"param1 {name} param3", "End of WHO.");
        }
    }
}
