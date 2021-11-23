namespace UniSpyServer.Servers.Chat.Test.General
{
    public class GeneralRequests
    {
        public const string CDKey = "CDKEY XXXX-XXXX-XXXX-XXXX\r\n";

        public const string Crypt = "CRYPT des 1 gmtest\r\n";

        public const string GetKey = "GETKEY spyguy 004 0 :\\b_firewall\\b_profileid\\b_ipaddress\\b_publicip\\b_privateip\\b_authresponse\\b_gamever\\b_val\r\n"; // \r\n CRLF

        public const string GetUdpRelay = "GETUDPRELAY\r\n";

        public const string Invite = "INVITE test spyguy\r\n";

        public const string ListLimit = "LISTLIMIT 5 test\r\n";

        public const string List = "LIST test\r\n";

        public const string LoginPreAuth = "LOGINPREAUTH xxxxx yyyyy\r\n";

        public const string LoginNickAndEmail = "LOGIN 0 * xxxxx :spyguyspyguyunispy.org\r\n"; // TODO: add binary data test [0D][0A]

        public const string LoginUniqueNick = "LOGIN 0 spyguy xxxxx\r\n";

        public const string Names = "NAMES\r\n";

        public const string Nick = "NICK :spyguy\r\n";

        public const string Ping = "PING\r\n"; // TODO: add binary data test [0D][0A]

        public const string Pong = "PONG :Pong!\r\n";

        public const string Quit = "QUIT :Later!\r\n"; // TODO: add binary data test [0D][0A]

        public const string RegisterNick = "REGISTERNICK 0 spyguy XXXX-XXXX-XXXX-XXXX\r\n";

        public const string SetGroup = "SETGROUP test\r\n";

        public const string SetKey = "SETKEY :test\r\n";

        public const string UserIP = "USRIP\r\n";

        public const string User = "USER spyguy 127.0.0.1 peerchat.unispy.org :spyguy2\r\n";

        public const string WhoIs = "WHOIS spyguy\r\n";

        public const string WhoChannelUsersInfo = "WHO #room\r\n";

        public const string WhoUserInfo = "WHO spyguy\r\n";
    }
}
