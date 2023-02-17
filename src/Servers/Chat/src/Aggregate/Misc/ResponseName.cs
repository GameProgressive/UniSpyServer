namespace UniSpy.Server.Chat.Aggregate.Misc
{
    public sealed class ResponseName
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
        public const string PrivateMsg = "PRIVMSG";
        /// <summary>
        /// Send a notice message
        /// </summary>
        public const string Notice = "NOTICE";
        /// <summary>
        /// Send an under the table message
        /// </summary>
        public const string UnderTheTableMsg = "UTM";
        /// <summary>
        /// Send an above the table message
        /// </summary>
        public const string AboveTheTableMsg = "ATM";
        public const string Ping = "PING";
        public const string Pong = "PONG";
        /// <summary>
        /// Search with nickname
        /// </summary>
        public const string Nick = "NICK";
        /// <summary>
        /// Join a channel
        /// </summary>
        public const string Join = "JOIN";
        /// <summary>
        /// Leave a channel
        /// </summary>
        public const string Part = "PART";
        /// <summary>
        /// Kick a user from a channel
        /// </summary>
        public const string Kick = "KICK";
        /// <summary>
        /// Quit irc chat server
        /// </summary>
        public const string Quit = "QUIT";

        public const string Kill = "KILL";
        /// <summary>
        /// Change channel topic
        /// </summary>
        public const string ChannelTopic = "TOPIC";
        /// <summary>
        /// Change channel mode
        /// </summary>
        public const string Mode = "MODE";

        public const string Error = "ERROR";
        /// <summary>
        /// Invite a user to a channel
        /// </summary>
        public const string Invite = "INVITE";
    }
}
