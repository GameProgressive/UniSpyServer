using Chat.Entity.Structure.ChatChannel;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatUser;
using Chat.Server;

namespace Chat.Entity.Structure.ChatResponse
{
    public class ChatReplyCode
    {
        public const string Welcome = "001";
        public const string UserIP = "302";
        public const string WhoIsUser = "311";
        public const string EndOfWho = "315";
        public const string EndOfWhoIs = "318";
        public const string WhoIsChannels = "319";
        public const string ListStart = "321";
        public const string List = "322";
        public const string ListEnd = "323";
        public const string ChannelModels = "324";
        public const string NoTopic = "331";
        public const string Topic = "332";
        public const string WhoReply = "352";
        public const string NameReply = "353";
        public const string EndOfNames = "366";
        public const string BanList = "367";
        public const string EndOfBanList = "368";
        public const string GetKey = "700";
        public const string EndGetKey = "701";
        public const string GetCKey = "702";
        public const string EndGetCKey = "703";
        public const string GetChanKey = "704";
        public const string SecureKey = "705";
        public const string CDKey = "706";
        public const string Login = "707";
        public const string GetUDPRelay = "712";

        public const string PRIVMSG = "PRIVMSG";
        public const string NOTICE = "NOTICE";
        public const string UTM = "UTM";
        public const string ATM = "ATM";
        public const string PING = "PING";
        public const string PONG = "PONG";
        public const string NICK = "NICK";
        public const string JOIN = "JOIN";
        public const string PART = "PART";
        public const string KICK = "KICK";
        public const string QUIT = "QUIT";
        public const string KILL = "KILL";
        public const string TOPIC = "TOPIC";
        public const string MODE = "MODE";
        public const string ERROR = "ERROR";
        public const string INVITE = "INVITE";
    }
}
