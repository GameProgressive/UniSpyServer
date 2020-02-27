using System;
namespace ServerBrowser.Entity.Enumerator
{
    public enum SBClientRequestType
    {
        ServerListRequest,
        ServerInfoRequest,
        SendMessageRequest,
        KeepAliveReply,
        MapLoopRequest,
        PlayerSearchRequest
    }
    public enum SBServerResponseType
    {
        PushKeysMessage = 1,
        PushServerMessage,
        KeepAliveMessage,
        DeleteServerMessage,
        MapLoopMessage,
        PlayerSearchMessage
    }
    public enum SBProtocolVersion
    {
        ListProtocolVersion1 = 0,
        ListEncodingVersion = 3
    }
    public enum SBServerListUpdateOption
    {
        SendFieldForAll = 1,
        NoServerList,
        PushUpdates = 4,
        SendGroups = 32,
        NoListCache = 64,
        LimitResultCount = 128
    }
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
}
