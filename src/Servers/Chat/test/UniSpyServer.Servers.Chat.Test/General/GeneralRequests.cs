namespace UniSpyServer.Servers.Chat.Test.General
{
    public class GeneralRequests
    {
        public const string CDKey = "CDKEY XXXX-XXXX-XXXX-XXXX";

        public const string Crypt = "CRYPT des 1 gmtest";

        public const string GetKey = @"GETKEY SpyGuy 004 0 :\b_firewall\b_profileid\b_ipaddress\b_publicip\b_privateip\b_authresponse\b_gamever\b_val" + "\r\n"; // \r\n CRLF

        public const string GetUdpRelay = "GETUDPRELAY";

        public const string Invite = "INVITE TestRoom Spyguy";

        public const string ListLimit = "LISTLIMIT 5 Test";

        public const string List = "LIST Test";

        public const string LoginPreAuth = "LOGINPREAUTH XXXXXXXXXX YYYYYYYYYY";

        public const string Login = "LOGIN 1 SpyGuy spyguy@gamespy.com SpyGuy XXXXXXXXXX";

        public const string Names = "NAMES";

        public const string Nick = "NICK Wiz";

        public const string Ping = "PING";

        public const string Pong = "PONG test";

        public const string Quit = "QUIT Bye";

        public const string RegisterNick = "REGISTERNICK 1 SpyGuy XXXX-XXXX-XXXX-XXXX";

        public const string SetGroup = "SETGROUP Test";

        public const string SetKey = "SETKEY";

        public const string UserIP = "USRIP";

        public const string User = "USER pants1 127.0.0.1 peerchat.gamespy.com :pants2";

        public const string WhoIs = "WHOIS SpyGuy";

        public const string Who = "WHO SpyGuy";
    }
}
