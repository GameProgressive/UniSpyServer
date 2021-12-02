namespace UniSpyServer.Servers.Chat.Test.General
{
    public class GeneralRequests
    {
        public const string CDKey = "CDKEY XXXX-XXXX-XXXX-XXXX";

        public const string Crypt = "CRYPT des 1 gmtest";

        public const string GetKey = "GETKEY spyguy 004 0 :\\b_firewall\\b_profileid\\b_ipaddress\\b_publicip\\b_privateip\\b_authresponse\\b_gamever\\b_val"; //  CRLF

        public const string GetUdpRelay = "GETUDPRELAY";

        public const string Invite = "INVITE test spyguy";

        public const string ListLimit = "LISTLIMIT 5 test";

        public const string List = "LIST test";

        public const string LoginPreAuth = "LOGINPREAUTH xxxxx yyyyy";

        public const string LoginNickAndEmail = "LOGIN 0 * xxxxx :spyguy@spyguy@unispy.org"; // TODO: add binary data test [0D][0A]

        public const string LoginUniqueNick = "LOGIN 0 spyguy xxxxx";

        public const string Names = "NAMES";

        public const string Nick = "NICK :spyguy";

        public const string Ping = "PING"; // TODO: add binary data test [0D][0A]

        public const string Pong = "PONG :Pong!";

        public const string Quit = "QUIT :Later!"; // TODO: add binary data test [0D][0A]

        public const string RegisterNick = "REGISTERNICK 0 spyguy XXXX-XXXX-XXXX-XXXX";

        public const string SetGroup = "SETGROUP test";

        public const string SetKey = "SETKEY :test";

        public const string UserIP = "USRIP";

        public const string User = "USER spyguy 127.0.0.1 peerchat.unispy.org :spyguy2";

        public const string WhoIs = "WHOIS spyguy";

        public const string WhoChannelUsersInfo = "WHO #room";

        public const string WhoUserInfo = "WHO spyguy";
    }
}
