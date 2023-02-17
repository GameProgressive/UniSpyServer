namespace UniSpy.Server.Chat.Enumerate
{
    public enum ChatErrorCode
    {
        Parse,
        DataOperation,
        ConstructResponse,
        NotChannelOperator,
        UserAlreadyInChannel,
        UnSupportedGame,
        NoError,
        IRCError,
    }
}