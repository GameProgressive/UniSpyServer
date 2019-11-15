namespace StatsAndTracking.Structure
{
    public class ChatRPL
    {
        const int Welcome = 001;
        const int USRIP = 302;
        const int WhoIsUser = 311;
        const int EndOfWho = 315;
        const int EndOfWhoIs = 318;
        const int WhoIsChannels = 319;
        const int ListStart = 321;
        const int List = 322;
        const int ListEnd = 323;
        const int ChannelModels = 324;
        const int NoToPic = 331;
        const int ToPic = 332;
        const int WhoReply = 352;
        const int NameReply = 353;
        const int EndOfNames = 366;
        const int BanList = 367;
        const int EndOfBanList = 368;
        const int GetKey = 700;
        const int EndGetKey = 701;
        const int GetCKey = 702;
        const int EndGetCKey = 703;
        const int GetCHANKey = 704;
        const int SecureKey = 705;
        const int CDKey = 706;
        const int Login = 707;
        const int GetUDPRelay = 712;
    }
    public class ChatError
    {
        const int NoSuchNICK = 401;
        const int NoSuchChannel = 403;
        const int TooManyChannels = 405;
        const int ErrOneUSNickName = 432;
        const int NickNameInUse = 433;
        const int ChannelIsFull = 471;
        const int InviteOnlyChan = 473;
        const int BannedFromChan = 474;
        const int BadChannelKey = 475;
        const int BadChanMask = 476;
        const int Login_Failed = 708;
        const int No_Unique_Nick = 709;
        const int Unique_NIick_Expired = 710;
        const int Register_Nick_Failed = 711;
    }
    public class ChatMode
    {
        const int End = 0;
        const int Ban = 1;
        const int Invite_Only= 2;
        const int LIMIT = 3;
        const int PRIVATE = 4;
        const int SECRET = 5;
        const int Key = 6;
        const int ModeRated = 7;
        const int No_EXTERNAL_MESSAGES = 8;
        const int ONLY_OPS_CHANGE_TOPIC = 9;
        const int OP = 10;
        const int Voice = 11;
        const int USERS_HIDDEN = 12;
        const int RECEIVE_WALLOPS = 13;
        const int OPS_OBEY_Channel_LIMIT = 14;
    }
    public class GSPeerChatCTX
    {
        public byte GSPeerChat1;
        public byte GSPeerChat2;
        public byte[] GSPeerChatCrypt = new byte[256];
    }

}
