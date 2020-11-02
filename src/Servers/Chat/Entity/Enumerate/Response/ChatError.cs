namespace Chat.Entity.Structure
{
    public enum ChatError
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
        UnSupportedGame
    }
}