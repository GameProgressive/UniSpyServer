namespace Chat.Entity.Structure
{
    public enum ChatErrorCode
    {
        //our customized error
        Parse,
        DataOperation,
        ConstructResponse,
        NotChannelOperator,
        UserAlreadyInChannel,
        NoError,
        IRCError,
        NoSuchNick,
        NickNameExisted,
        NoSuchChannel,
        UnSupportedGame,
        JoinedTooManyChannel,
        BannedByChannel,
        ChannelFull,
    }
}