using System;
using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
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
