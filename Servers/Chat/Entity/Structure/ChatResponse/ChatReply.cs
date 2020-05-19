using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatUser;

namespace Chat.Entity.Structure.ChatResponse
{
    public class ChatReply
    {
        private const string Welcome = "001";
        private const string UserIP = "302";
        private const string WhoIsUser = "311";
        private const string EndOfWho = "315";
        private const string EndOfWhoIs = "318";
        private const string WhoIsChannels = "319";
        private const string ListStart = "321";
        private const string List = "322";
        private const string ListEnd = "323";
        private const string ChannelModels = "324";
        private const string NoTopic = "331";
        private const string Topic = "332";
        private const string WhoReply = "352";
        private const string NameReply = "353";
        private const string EndOfNames = "366";
        private const string BanList = "367";
        private const string EndOfBanList = "368";
        private const string GetKey = "700";
        private const string EndGetKey = "701";
        private const string GetCKey = "702";
        private const string EndGetCKey = "703";
        private const string GetChanKey = "704";
        private const string SecureKey = "705";
        private const string CDKey = "706";
        private const string Login = "707";
        private const string GetUDPRelay = "712";

        private const string PRIVMSG = "PRIVMSG";
        private const string NOTICE = "NOTICE";
        private const string UTM = "UTM";
        private const string ATM = "ATM";
        private const string PING = "PING";
        private const string PONG = "PONG";
        private const string NICK = "NICK";
        private const string JOIN = "JOIN";
        private const string PART = "PART";
        private const string KICK = "KICK";
        private const string QUIT = "QUIT";
        private const string KILL = "KILL";
        private const string TOPIC = "TOPIC";
        private const string MODE = "MODE";
        private const string ERROR = "ERROR";
        private const string INVITE = "INVITE";


        public static string BuildWelcomeReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(
                  Welcome, userInfo.NickName, "Welcome to RetrosSpy!");
        }

        public static string BuildCryptReply(string clientKey, string serverKey)
        {
            return ChatCommandBase.BuildReply(
                    SecureKey,
                    $"* {clientKey} {serverKey}");
        }
        public static string BuildPingReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(PONG);
        }

        public static string BuildUserIPReply(string ip)
        {
            return ChatCommandBase.BuildReply(UserIP, "", $"@{ip}");
        }

        public static string BuildWhoReply(string channelName, ChatUserInfo userInfo, string modes)
        {
            return ChatCommandBase.BuildReply(
                    WhoReply,
                    $"* {channelName} " +
                    $"{userInfo.UserName} {userInfo.PublicIPAddress} * {userInfo.NickName} {modes} param7");
        }

        public static string BuildEndOfWhoReply(ChatUserInfo userInfo)
        {
            return ChatCommandBase.BuildReply(EndOfWho, $"* {userInfo.NickName} param3");
        }

        public static string BuildWhoIsUserReply(ChatUserInfo userInfo)
        {
            return ChatCommandBase.BuildReply(
                WhoIsUser,
                 $"{userInfo.NickName} {userInfo.Name} {userInfo.UserName} {userInfo.PublicIPAddress} *",
                 userInfo.UserName);
        }
        public static string BuildWhoIsChannelReply(ChatUserInfo userInfo, string channelName)
        {
            return ChatCommandBase.BuildReply(
                    WhoIsChannels,
                    $"{userInfo.NickName} {userInfo.Name}",
                    channelName
                    );
        }

        public static string BuildEndOfWhoIsReply(ChatUserInfo userInfo)
        {
            return ChatCommandBase.BuildReply(
                    EndOfWhoIs,
                    $"{userInfo.NickName} {userInfo.Name}",
                    "End of /WHOIS list."
                    );
        }

        public static string BuildGetCKeyReply(ChatChannelUser user, string channelName, string cookie, string flags)
        {
            return user.BuildReply(GetCKey,
                $"* {channelName} {user.UserInfo.NickName} {cookie} {flags}");
        }

        public static string BuildEndOfGetCKeyReply(ChatChannelUser user, string channelName, string cookie)
        {
            return user.BuildReply(EndGetCKey,
                  $"* {channelName} {cookie}",
                  "End Of /GETCKEY.");
        }

        public static string BuildKickReply(string channelName, ChatChannelUser kicker, ChatChannelUser kickee, string reason)
        {
            return kicker.BuildReply(KICK,
                $"KICK {channelName} {kicker.UserInfo.NickName} {kickee.UserInfo.NickName}",
               reason);
        }

        public static string BuildModeReply(ChatChannelUser user, string channelName, string modes)
        {
            return user.BuildReply(MODE, $"{channelName} {modes}");
        }

        public static string BuildListStartReply(ChatUserInfo userInfo, ChatChannelProperty property)
        {
            return userInfo.BuildReply(ListStart,
                      $"param1 {property.ChannelName} {property.ChannelUsers.Count} {property.ChannelTopic}");
        }
        public static string BuildListEndReply(ChatUserInfo userInfo)
        {
            return userInfo.BuildReply(ListEnd);
        }

        public static string BuildNoticeReply(ChatUserInfo info, string targetName, string message)
        {
            return info.BuildReply(NOTICE, $"{targetName}", message);
        }

        public static string BuildPrivMsgReply(ChatUserInfo info, string targetName, string message)
        {
            return info.BuildReply(PRIVMSG, $"{targetName}", message);
        }

        public static string BuildUTMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(UTM, name, message);
        }

        public static string BuildJoinReply(ChatChannelUser user, string channelName)
        {
            return user.BuildReply(JOIN, channelName);
        }

        public static string BuildPartReply(ChatChannelUser user, string channelName, string reason)
        {
            return user.BuildReply(PART, channelName, reason);
        }

        public static string BuildNameReply(string nickName, string channelName, string nicks)
        {
            return ChatCommandBase.BuildReply(
                    NameReply,
                    $"{nickName} = {channelName}", nicks);
        }

        public static string BuildEndOfNameReply(string nickName, string channelName)
        {
            return ChatCommandBase.BuildReply(EndOfNames,
                    $"{nickName} {channelName}", @"End of /NAMES list.");
        }

        public static string BuildChannelModesReply(ChatChannelUser user, string channelName, string modes)
        {
            return user.BuildReply(ChannelModels,
                $"{user.UserInfo.NickName} {channelName} {modes}");
        }

        public static string BuildATMReply(ChatUserInfo info, string name, string message)
        {
            return info.BuildReply(ATM, $"{name} {message}");
        }

        public static string BuildNoTopicReply(string channelName)
        {
            return
                ChatCommandBase.BuildReply(NoTopic, channelName);
        }
        public static string BuildTopicReply(string channelName, string channelTopic)
        {
            return
                   ChatCommandBase.BuildReply(TOPIC, channelName, channelTopic);
        }

        public static string BuildLoginReply(uint userid, uint profileid)
        {
            return ChatCommandBase.BuildReply(Login, $"param1 {userid} {profileid}");
        }

        public static string BuildGetChanKeyReply(ChatChannelUser user, string channelName, string cookie, string flags)
        {
            return user.BuildReply(GetChanKey, $"param1 {channelName} {cookie} {flags}");
        }

        public static string BuildGetKeyReply(ChatUserInfo info, string cookie, string flags)
        {
            return info.BuildReply(GetKey, $"param1 {info.NickName} {cookie} {flags}");
        }
        public static string BuildEndOfGetKeyReply(ChatUserInfo info, string cookie)
        {
            return info.BuildReply(EndGetKey, $"param1 param2 {cookie} param4","End of GETKEY.");
        }
    }
}
