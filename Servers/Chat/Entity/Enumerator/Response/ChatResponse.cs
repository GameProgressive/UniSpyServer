namespace Chat.Entity.Structure
{
    /// <summary>
    /// response code
    /// </summary>
    public enum ChatResponseType
    {
        Welcome = 001,
        UserIP = 302,
        WhoIsUser = 311,
        EndOfWho = 315,
        EndOfWhoIs = 318,
        WhoIsChannels = 319,
        ListStart = 321,
        List = 322,
        ListEnd = 323,
        ChannelModels = 324,
        NoTopic = 331,
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
}
