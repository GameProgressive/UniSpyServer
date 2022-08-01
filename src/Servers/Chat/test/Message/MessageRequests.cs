namespace UniSpyServer.Servers.Chat.Test.Message
{
    public class MessageRequests
    {
        public const string AboveTheTableMsg = "ATM #GSP!room!test :hello this is a test.";

        public const string Notice = "NOTICE #GSP!room!test :hello this is a test.";

        public const string PrivateMsg = "PRIVMSG #GSP!room!test :hello this is a test.";

        public const string UnderTheTableMsg = "UTM #GSP!room!test :hello this is a test.";

        public const string ActionMsg = "PRIVMSG #GSP!room!test :\001ACTION hello this is a test.\001";
    }
}
