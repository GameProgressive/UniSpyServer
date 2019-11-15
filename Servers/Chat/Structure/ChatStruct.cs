namespace RetroSpyServer.Servers.Chat.Structures
{
    public class GSPeerChatCTX
    {
        public byte GSPeerChat1;
        public byte GSPeerChat2;
        public byte[] GSPeerChatCrypt = new byte[256];
    }
    /// <summary>
    /// RPL is remote program load
    /// </summary>
    public class ChatCode
    {
        const int RPL_WELCOME = 001;
        const int RPL_USRIP = 302;
        const int RPL_WHOISUSER = 311;
        const int RPL_ENDOFWHO = 315;
        const int RPL_ENDOFWHOIS = 318;
        const int RPL_WHOISCHANNELS = 319;
        const int RPL_LISTSTART = 321;
        const int RPL_LIST = 322;
        const int RPL_LISTEND = 323;
        const int RPL_CHANNELMODEIS = 324;
        const int RPL_NOTOPIC = 331;
        const int RPL_TOPIC = 332;
        const int RPL_WHOREPLY = 352;
        const int RPL_NAMEREPLY = 353;
        const int RPL_ENDOFNAMES = 366;
        const int RPL_BANLIST = 367;
        const int RPL_ENDOFBANLIST = 368;
        const int RPL_GETKEY = 700;
        const int RPL_ENDGETKEY = 701;
        const int RPL_GETCKEY = 702;
        const int RPL_ENDGETCKEY = 703;
        const int RPL_GETCHANKEY = 704;
        const int RPL_SECUREKEY = 705;
        const int RPL_CDKEY = 706;
        const int RPL_LOGIN = 707;
        const int RPL_GETUDPRELAY = 712;

        const int ERR_NOSUCHNICK = 401;
        const int ERR_NOSUCHCHANNEL = 403;
        const int ERR_TOOMANYCHANNELS = 405;
        const int ERR_ERRONEUSNICKNAME = 432;
        const int ERR_NICKNAMEINUSE = 433;
        const int ERR_CHANNELISFULL = 471;
        const int ERR_INVITEONLYCHAN = 473;
        const int ERR_BANNEDFROMCHAN = 474;
        const int ERR_BADCHANNELKEY = 475;
        const int ERR_BADCHANMASK = 476;
        const int ERR_LOGIN_FAILED = 708;
        const int ERR_NO_UNIQUE_NICK = 709;
        const int ERR_UNIQUE_NICK_EXPIRED = 710;
        const int ERR_REGISTER_NICK_FAILED = 711;

        const int MODE_END = 0;
        const int MODE_BAN = 1;
        const int MODE_INVITE_ONLY = 2;
        const int MODE_LIMIT = 3;
        const int MODE_PRIVATE = 4;
        const int MODE_SECRET = 5;
        const int MODE_KEY = 6;
        const int MODE_MODERATED = 7;
        const int MODE_NO_EXTERNAL_MESSAGES = 8;
        const int MODE_ONLY_OPS_CHANGE_TOPIC = 9;
        const int MODE_OP = 10;
        const int MODE_VOICE = 11;
        const int MODE_USERS_HIDDEN = 12;
        const int MODE_RECEIVE_WALLOPS = 13;
        const int MODE_OPS_OBEY_CHANNEL_LIMIT = 14;
    }
}
