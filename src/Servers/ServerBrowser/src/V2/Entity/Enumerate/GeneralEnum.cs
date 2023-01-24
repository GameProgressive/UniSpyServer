namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Enumerate
{
    public enum PlayerSearchOptions
    {
        SearchAllGames = 1,
        SearchLeftSubstring = 2,
        SearchRightSubString = 4,
        SearchAnySubString = 8,
    }

    public enum QueryType
    {
        Basic,
        Full,
        ICMP
    }

    public enum DataKeyType
    {
        String,
        Byte,
        Short
    }
}
