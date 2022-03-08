namespace UniSpyServer.Servers.ServerBrowser.Entity.Enumerate
{
    public enum RequestType : byte
    {
        ServerListRequest,
        ServerInfoRequest,
        SendMessageRequest,
        KeepAliveReply,
        MapLoopRequest,
        PlayerSearchRequest,
        NatNegRequest = 30,
    }

    public enum ResponseType
    {
        PushKeysMessage = 1,
        PushServerMessage,
        KeepAliveMessage,
        DeleteServerMessage,
        MapLoopMessage,
        PlayerSearchMessage
    }

    public enum ProtocolVersion
    {
        ListProtocolVersion1 = 0,
        ListEncodingVersion = 3
    }

    public enum ServerListUpdateOption
    {
        /// <summary>
        /// This is used to tell server browser client want main server list (keys and values)
        /// </summary>
        ServerMainList = 0,
        SendFieldForAll = 1,
        /// <summary>
        /// get the full information of all servers
        /// </summary>
        ServerFullInfoList = 2,
        /// <summary>
        /// Push new local game status to QueryReport server
        /// </summary>
        P2PServerMainList = 4,
        AlternateSourceIP = 8,
        P2PGroupRoomList = 32,
        NoListCache = 64,
        LimitResultCount = 128
    }

    public enum GameServerFlags
    {
        /// <summary>
        /// game can directly send request to dedicate server
        /// </summary>
        UnsolicitedUDPFlag = 1,
        /// <summary>
        /// private ip exist
        /// </summary>
        PrivateIPFlag = 2,
        /// <summary>
        /// connect with nat neg
        /// </summary>
        ConnectNegotiateFlag = 4,
        /// <summary>
        /// server has icmp
        /// </summary>
        ICMPIPFlag = 8,
        /// <summary>
        /// non standard query port
        /// </summary>
        NonStandardPort = 16,

        /// <summary>
        /// nonstandard private port
        /// </summary>
        NonStandardPrivatePortFlag = 32,
        /// <summary>
        /// has standard keys
        /// </summary>
        HasKeysFlag = 64,
        /// <summary>
        /// has full rules keys
        /// </summary>
        HasFullRulesFlag = 128
    }
}
