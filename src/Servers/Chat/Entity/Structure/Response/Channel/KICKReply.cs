using Chat.Entity.Structure.ChannelInfo;

namespace Chat.Entity.Structure.Response.Channel
{
    public class KICKReply
    {
        public static string BuildKickReply(string channelName, ChatChannelUser kicker, ChatChannelUser kickee, string reason)
        {
            return kicker.BuildReply(ChatReplyCode.KICK,
                $"KICK {channelName} {kicker.UserInfo.NickName} {kickee.UserInfo.NickName}",
               reason);
        }
    }
}
