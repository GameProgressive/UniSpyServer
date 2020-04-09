namespace Chat.Entity.Structure
{
    public enum ChatType
    {
        List,
        Join,
        Topic,
        Names,
        WhoIs,
        CMode,
        UMode,
        Ban,
        GetBan,
        Nick,
        Who,
        CWho,
        GetKey,
        GetCKey,
        GgetCHANKEY,
        UnQuiet,
        CDKey,
        GetUDPRelay,
        NumTypes
    }
    /// <summary>
    /// response code
    /// </summary>
    public enum ChatRPL
    {
        Welcome = 001,
        USRIP = 302,
        WhoIsUser = 311,
        EndOfWho = 315,
        EndOfWhoIs = 318,
        WhoIsChannels = 319,
        ListStart = 321,
        List = 322,
        ListEnd = 323,
        ChannelModels = 324,
        NoToPic = 331,
        ToPic = 332,
        WhoReply = 352,
        NameReply = 353,
        EndOfNames = 366,
        BanList = 367,
        EndOfBanList = 368,
        GetKey = 700,
        EndGetKey = 701,
        GetCKey = 702,
        EndGetCKey = 703,
        GetCHANKey = 704,
        SecureKey = 705,
        CDKey = 706,
        Login = 707,
        GetUDPRelay = 712
    }

    public enum ChatError
    {
        NoSuchNICK = 401,
        NoSuchChannel = 403,
        TooManyChannels = 405,
        ErrOneUSNickName = 432,
        NickNameInUse = 433,
        MoreParameters = 461,
        ChannelIsFull = 471,
        InviteOnlyChan = 473,
        BannedFromChan = 474,
        BadChannelKey = 475,
        BadChanMask = 476,
        Login_Failed = 708,
        No_Unique_Nick = 709,
        Unique_NIick_Expired = 710,
        Register_Nick_Failed = 711,
        Parse = 1024,
        DataOperation,
        ConstructResponse,
        NoError
    }

    public enum ChatMode
    {
        End = 0,
        Ban = 1,
        Invite_Only = 2,
        LIMIT = 3,
        PRIVATE = 4,
        SECRET = 5,
        Key = 6,
        ModeRated = 7,
        No_EXTERNAL_MESSAGES = 8,
        ONLY_OPS_CHANGE_TOPIC = 9,
        OP = 10,
        Voice = 11,
        USERS_HIDDEN = 12,
        RECEIVE_WALLOPS = 13,
        OPS_OBEY_Channel_LIMIT = 14,
    }
}
