using Chat.Entity.Structure.ChannelInfo;
using Chat.Entity.Structure.User;

namespace Chat.Entity.Structure.Response.General
{
    public class LISTReply
    {
        public static string BuildListStartReply(ChatUserInfo userInfo, ChatChannelProperty property)
        {
            return userInfo.BuildReply(ChatReplyCode.ListStart,
                      $"param1 {property.ChannelName} {property.ChannelUsers.Count} {property.ChannelTopic}");
        }
        public static string BuildListEndReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(ChatReplyCode.ListEnd);
        }
    }
}
