namespace Chat.Entity.Structure
{
    public enum ChatError
    {
        //our customized error
        Parse,
        DataOperation,
        ConstructResponse,
        NotChannelOperator,
        NoError,
        IRCError,
    }
}