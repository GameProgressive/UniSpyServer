using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Response.General
{
    public class WHOReply
    {
        public static string BuildWhoReply(string channelName, ChatUserInfo userInfo, string modes)
        {
            return ChatResponseBase.BuildRPL(
                    ChatReplyCode.WhoReply,
                    $"param1 {channelName} " +
                    $"{userInfo.UserName} {userInfo.PublicIPAddress} param5 {userInfo.NickName} {modes} param8");
        }

        public static string BuildEndOfWhoReply(string name)
        {
            return ChatResponseBase.BuildRPL(ChatReplyCode.EndOfWho, $"param1 {name} param3", "End of WHO.");
        }
    }
}
