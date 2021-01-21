namespace Chat.Entity.Structure.Misc
{
    public class ChatReplyName
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

        /// <summary>
        /// Send a private message
        /// </summary>
        public const string PRIVMSG = "PRIVMSG";
        /// <summary>
        /// Send a notice message
        /// </summary>
        public const string NOTICE = "NOTICE";
        /// <summary>
        /// Send an under the table message
        /// </summary>
        public const string UTM = "UTM";
        /// <summary>
        /// Send an above the table message
        /// </summary>
        public const string ATM = "ATM";
        public const string PING = "PING";
        public const string PONG = "PONG";
        /// <summary>
        /// Search with nickname
        /// </summary>
        public const string NICK = "NICK";
        /// <summary>
        /// Join a channel
        /// </summary>
        public const string JOIN = "JOIN";
        /// <summary>
        /// Leave a channel
        /// </summary>
        public const string PART = "PART";
        /// <summary>
        /// Kick a user from a channel
        /// </summary>
        public const string KICK = "KICK";
        /// <summary>
        /// Quit irc chat server
        /// </summary>
        public const string QUIT = "QUIT";

        public const string KILL = "KILL";
        /// <summary>
        /// Change channel topic
        /// </summary>
        public const string TOPIC = "TOPIC";
        /// <summary>
        /// Change channel mode
        /// </summary>
        public const string MODE = "MODE";

        public const string ERROR = "ERROR";
        /// <summary>
        /// Invite a user to a channel
        /// </summary>
        public const string INVITE = "INVITE";
    }
}
