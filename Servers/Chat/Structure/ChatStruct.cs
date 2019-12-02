namespace StatsAndTracking.Structure
{
    public class ChatRPL
    {
        public const int Welcome = 001;
        public const int USRIP = 302;
        public const int WhoIsUser = 311;
        public const int EndOfWho = 315;
        public const int EndOfWhoIs = 318;
        public const int WhoIsChannels = 319;
        public const int ListStart = 321;
        public const int List = 322;
        public const int ListEnd = 323;
        public const int ChannelModels = 324;
        public const int NoToPic = 331;
        public const int ToPic = 332;
        public const int WhoReply = 352;
        public const int NameReply = 353;
        public const int EndOfNames = 366;
        public const int BanList = 367;
        public const int EndOfBanList = 368;
        public const int GetKey = 700;
        public const int EndGetKey = 701;
        public const int GetCKey = 702;
        public const int EndGetCKey = 703;
        public const int GetCHANKey = 704;
        public const int SecureKey = 705;
        public const int CDKey = 706;
        public const int Login = 707;
        public const int GetUDPRelay = 712;
    }
    public class ChatError
    {
        public const int NoSuchNICK = 401;
        public const int NoSuchChannel = 403;
        public const int TooManyChannels = 405;
        public const int ErrOneUSNickName = 432;
        public const int NickNameInUse = 433;
        public const int MoreParameters = 461;
        public const int ChannelIsFull = 471;
        public const int InviteOnlyChan = 473;
        public const int BannedFromChan = 474;
        public const int BadChannelKey = 475;
        public const int BadChanMask = 476;
        public const int Login_Failed = 708;
        public const int No_Unique_Nick = 709;
        public const int Unique_NIick_Expired = 710;
        public const int Register_Nick_Failed = 711;
    }
    public class ChatMode
    {
        public const int End = 0;
        public const int Ban = 1;
        public const int Invite_Only= 2;
        public const int LIMIT = 3;
        public const int PRIVATE = 4;
        public const int SECRET = 5;
        public const int Key = 6;
        public const int ModeRated = 7;
        public const int No_EXTERNAL_MESSAGES = 8;
        public const int ONLY_OPS_CHANGE_TOPIC = 9;
        public const int OP = 10;
        public const int Voice = 11;
        public const int USERS_HIDDEN = 12;
        public const int RECEIVE_WALLOPS = 13;
        public const int OPS_OBEY_Channel_LIMIT = 14;
    }
    public class GSPeerChatCTX
    {
        public byte GSPeerChat1;
        public byte GSPeerChat2;
        public byte[] GSPeerChatCrypt = new byte[256];
    }

}
