namespace UniSpy.Server.Chat.Test.Channel
{
    public class ChannelRequests
    {
        public const string GetChannelKey = "GETCHANKEY #GSP!room!test 0000 0 :\\username\\nickname\0";
        public const string GetCKeyChannelSpecificUser = "GETCKEY #GSP!room!test spyguy 0000 0 :\\username\\nickname\0";
        public const string GetCKeyChannelAllUser = "GETCKEY #GSP!room!test * 0000 0 :\\username\\nickname\0";

        public const string Join = "JOIN #GSP!room!test";

        public const string JoinWithPass = "JOIN #GSP!room!test pass123";

        public const string Kick = "KICK #islabul spyguy :Spam";

        public const string ModeChannel = "MODE #GSP!room!test +l 2";
        public const string ModeUser = "MODE spyguy +s";

        public const string Part = "PART #GSP!room!test :test";

        public const string SetChannelKey = "SETCHANNELKEY #GSP!room!test 0000 0:\\b_flags\\sh\0";

        public const string SetCKey = "SETCKEY #GSP!room!test spyguy 0000 0:\\b_flags\\sh\0";

        public const string TopicGetChannelTopic = "TOPIC #GSP!room!test";

        public const string TopicSetChannelTopic = "TOPIC #GSP!room!test :This is a topic message.";
    }
}
