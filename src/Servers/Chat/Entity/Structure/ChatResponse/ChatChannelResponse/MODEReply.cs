using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatChannel;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class MODEReply
    {
        public static string BuildModeReply(string channelName, string modes)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.MODE, $"{channelName} {modes}");
        }

        public static string BuildChannelModesReply(ChatChannelUser user, string channelName, string modes)
        {
            return ChatReplyBase.BuildReply(ChatReplyCode.ChannelModels,
                $"{user.UserInfo.NickName} {channelName} {modes}");
        }
    }
}
