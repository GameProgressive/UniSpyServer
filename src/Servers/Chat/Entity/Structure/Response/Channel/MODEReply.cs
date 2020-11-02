using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatChannel;

namespace Chat.Entity.Structure.Response.General
{
    public class MODEReply
    {
        public static string BuildModeReply(string channelName, string modes)
        {
            return ChatResponseBase.BuildResponse(ChatReplyCode.MODE, $"{channelName} {modes}");
        }

        public static string BuildChannelModesReply(ChatChannelUser user, string channelName, string modes)
        {
            return ChatResponseBase.BuildResponse(ChatReplyCode.ChannelModels,
                $"{user.UserInfo.NickName} {channelName} {modes}");
        }
    }
}
