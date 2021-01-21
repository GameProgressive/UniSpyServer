using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;

namespace Chat.Entity.Structure.Response.General
{
    public class MODEReply
    {
        public static string BuildModeReply(string channelName, string modes)
        {
            return ChatResponseBase.BuildRPL(ChatReplyName.MODE, $"{channelName} {modes}");
        }

        public static string BuildChannelModesReply(ChatChannelUser user, string channelName, string modes)
        {
            return ChatResponseBase.BuildRPL(ChatReplyName.ChannelModels,
                $"{user.UserInfo.NickName} {channelName} {modes}");
        }
    }
}
