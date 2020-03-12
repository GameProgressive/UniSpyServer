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
        NoServerList = 2,
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
    public enum SBKeyType
    {
        String,
        Byte,
        Short
    }
    public enum GameServerFlags
    {
        ServerEnd = 0,
        UnsolicitedUdpFlag = 1,
        PrivateIpFlag = 2,
        ConnectNegotiateFlag = 4,
        ICMPIpFlag = 8,
        NonStandardPrivatePortFlag = 32,
        HasKeysFlag = 64,
        HasFullRulesFlag = 128
    }
}
