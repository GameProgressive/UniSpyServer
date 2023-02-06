namespace UniSpyServer.UniSpyLib.Extensions
{
    public enum DbNumber : int
    {
        PeerGroup = 0,
        GameServer = 1,
        NatAddressInfo = 2,
        NatFailInfo = 3,
        GameTrafficRelay = 4
    }

    /// <summary>
    /// This class wraps functions that allow us to access
    /// the string representation of keys and values in Redis database
    /// </summary>
}
