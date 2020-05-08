using Chat.Entity.Structure.ChatChannel;

namespace Chat.Entity.Structure.ChatCommand
{
    public class KICK : ChatChannelCommandBase
    {
        public string NickName { get; protected set; }
        public string Reason { get; protected set; }

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
                return false;
            if (_cmdParams.Count != 2)
            {
                return false;
            }
            NickName = _cmdParams[0];
            if (_longParam == null)
                return false;
            Reason = _longParam;
            return true;
        }

        public static string BuildKickMessage(ChatChannelBase channel, ChatChannelUser kicker, ChatChannelUser kickee, string reason)
        {
            return BuildMessageRPL(
                $"{channel.Property.ChannelName} {kicker.UserInfo.NickName} {kickee.UserInfo.NickName}",
                reason);
        }
    }
}
