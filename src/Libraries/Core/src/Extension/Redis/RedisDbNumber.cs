namespace UniSpy.Server.Core.Extension.Redis
{
    public enum RedisDbNumber : int
    {
        PeerGroup = 0,
        GameServerV1 = 1,
        GameServerV2 = 2,
        NatAddressInfo = 3,
        NatFailInfo = 4,
        GameTrafficRelay = 5,
        ChatChannel = 6
    }
}
