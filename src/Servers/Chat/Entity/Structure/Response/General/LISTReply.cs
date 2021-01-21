using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Response.General
{
    public class LISTReply
    {
        public static string BuildListStartReply(ChatUserInfo userInfo, ChatChannelProperty property)
        {
            return userInfo.BuildReply(ChatReplyName.ListStart,
                      $"param1 {property.ChannelName} {property.ChannelUsers.Count} {property.ChannelTopic}");
        }
        public static string BuildListEndReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(ChatReplyName.ListEnd);
        }
    }
}
