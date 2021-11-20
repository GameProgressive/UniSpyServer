namespace UniSpyServer.Servers.Chat.Test.Channel
{
    public class ChannelRequests
    {
        public const string GetChannelKey = "GETCHANKEY XXXX";

        public const string GetCKeyChannelSpecificUser = @"GETCKEY #GSP!room!test spyguy 000 0 :\username\b_flags[0D][0A]";

        public const string GetCKeyChannelAllUser = @"GETCKEY #GSP!room!test * 000 0 :\username\b_flags[0D][0A]";

        public const string Join = "JOIN #GSP!room!test";

        public const string JoinWithPass = "JOIN #GSP!room!test pass123";

        public const string Kick = "KICK spyguy :Spam";

        public const string Mode = "MODE #GSP!room!test +l 2";

        public const string Part = "PART #GSP!room!test :test";

        public const string SetChannelKey = "SETCHANNELKEY";

        public const string SetCKey = @"SETCKEY #GSP!room!test spyguy :\b_flags\sh[0D][0A]";

        public const string TopicGetChannelTopic = "TOPIC #GSP!room!test";

        public const string TopicSetChannelTopic = "TOPIC #GSP!room!test :This is a topic message.";
    }
}
