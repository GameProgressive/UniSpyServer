
namespace ServerBrowser.Entity.Enumerator
{
    public enum PlayerSearchOptions
    {
        SearchAllGames = 1,
        SearchLeftSubstring,
        SearchRightSubString = 4,
        SearchAnySubString = 8,
    }
    public enum SBQueryType
    {
        Basic,
        Full,
        ICMP
    }
    public enum SBErrorCode
    {
        Parse,
        DataOperation,
        Construct,
        NoError
    }
    public enum SBKeyType
    {
        String,
        Byte,
        Short
    }

}
