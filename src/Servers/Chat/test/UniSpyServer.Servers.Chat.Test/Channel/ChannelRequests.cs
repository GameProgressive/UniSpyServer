namespace UniSpyServer.Servers.Chat.Test.Channel
{
    public class ChannelRequests
    {
        public const string GetChannelKey = "GETCHANKEY XXXX\r\n";

        public const string GetCKeyChannelSpecificUser = "GETCKEY #GSP!room!test spyguy 000 0 :\\username\\b_flags\r\n";

        public const string GetCKeyChannelAllUser = "GETCKEY #GSP!room!test * 000 0 :\\username\\b_flags\r\n";

        public const string Join = "JOIN #GSP!room!testr\r\n";

        public const string JoinWithPass = "JOIN #GSP!room!test pass123\r\n";

        public const string Kick = "KICK spyguy :Spam\r\n";

        public const string Mode = "MODE #GSP!room!test +l 2\r\n";

        public const string Part = "PART #GSP!room!test :test\r\n";

        public const string SetChannelKey = "SETCHANNELKEY\r\n";

        public const string SetCKey = "SETCKEY #GSP!room!test spyguy :\\b_flags\\sh\r\n";

        public const string TopicGetChannelTopic = "TOPIC #GSP!room!test\r\n";

        public const string TopicSetChannelTopic = "TOPIC #GSP!room!test :This is a topic message.\r\n";
    }
}
