namespace Chat.Entity.Structure
{
    public enum ChatMode : uint
    {
        End = 0,
        Ban = 1,
        InviteOnly = 2,
        Limit = 3,
        Private = 4,
        Secret = 5,
        Key = 6,
        ModeRated = 7,
        NoExternalMessages = 8,
        OlnyOPSChangeTopic = 9,
        OP = 10,
        Voice = 11,
        UsersHidden = 12,
        ReceiveWallOPS = 13,
        OPSObeyChannelLimit = 14,
    }
}
