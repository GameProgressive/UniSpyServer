using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatUser;
using System.Linq;

namespace Chat.Entity.Structure.ChatResponse.ChatGeneralResponse
{
    public class WHOISReply
    {
        public static string BuildWhoIsUserReply(ChatUserInfo userInfo)
        {
            return ChatReplyBase.BuildReply(
                ChatReplyCode.WhoIsUser,
                 $"{userInfo.NickName} {userInfo.Name} {userInfo.UserName} {userInfo.PublicIPAddress} *",
                 userInfo.UserName)
                +
                BuildJoinedChannelReply(userInfo)
                +
                BuildEndOfWhoIsReply(userInfo);
        }

        private static string BuildJoinedChannelReply(ChatUserInfo userInfo)
        {
            string buffer = "";
            if (userInfo.JoinedChannels.Count() != 0)
            {
                string channelNames = "";
                //todo remove last space
                foreach (var channel in userInfo.JoinedChannels)
                {
                    channelNames += $"@{channel.Property.ChannelName} ";
                }

                buffer +=
                    BuildWhoIsChannelReply(userInfo, channelNames);
            }
            return buffer;
        }

        private static string BuildWhoIsChannelReply(ChatUserInfo userInfo, string channelName)
        {
            return ChatReplyBase.BuildReply(
                    ChatReplyCode.WhoIsChannels,
                    $"{userInfo.NickName} {userInfo.Name}",
                    channelName
                    );
        }

        private static string BuildEndOfWhoIsReply(ChatUserInfo userInfo)
        {
            return ChatReplyBase.BuildReply(
                    ChatReplyCode.EndOfWhoIs,
                    $"{userInfo.NickName} {userInfo.Name}",
                    "End of /WHOIS list."
                    );
        }
    }
}
