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
        /// <summary>
        /// This is used to tell server browser client want main server list (keys and values)
        /// </summary>
        SendRequestedField = 0,
        SendFieldForAll = 1,
        /// <summary>
        /// this is used to check the connection to server browser
        /// </summary>
        NoServerList = 2,
        PushUpdates = 4,
        AlternateSourceIP = 8,
        SendGroups = 32,
        NoListCache = 64,
        LimitResultCount = 128
    }

    public enum GameServerFlags
    {
        UnsolicitedUDPFlag = 1,
        PrivateIPFlag = 2,
        ConnectNegotiateFlag = 4,
        ICMPIPFlag = 8,
        NonStandardPort = 16,
        NonStandardPrivatePortFlag = 32,
        HasKeysFlag = 64,
        HasFullRulesFlag = 128
    }
}
