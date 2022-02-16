namespace UniSpyServer.UniSpyLib.Extensions
{
    public enum DbNumber : int
    {
        PeerGroup = 0,
        GameServer = 1,
        NatNeg = 2,
        GamePresence = 3
    }

    /// <summary>
    /// This class wraps functions that allow us to access
    /// the string representation of keys and values in Redis database
    /// </summary>
}
