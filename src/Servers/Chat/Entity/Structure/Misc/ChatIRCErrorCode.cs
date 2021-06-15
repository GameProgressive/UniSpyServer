namespace Chat.Entity.Structure.Misc
{
    internal sealed class ChatIRCErrorCode
    {
        //GameSpy standard irc error
        public const string NoSuchNick = "401";
        public const string NoSuchChannel = "403";
        public const string TooManyChannels = "405";
        public const string ErrOneUSNickName = "432";
        public const string NickNameInUse = "433";
        public const string MoreParameters = "461";
        public const string ChannelIsFull = "471";
        public const string InviteOnlyChan = "473";
        public const string BannedFromChan = "474";
        public const string BadChannelKey = "475";
        public const string BadChanMask = "476";
        public const string LoginFailed = "708";
        public const string NoUniqueNick = "709";
        public const string UniqueNIickExpired = "710";
        public const string RegisterNickFailed = "711";
    }
}
