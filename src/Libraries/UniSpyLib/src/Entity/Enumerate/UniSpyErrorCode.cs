namespace UniSpyServer.UniSpyLib.Entity.Enumerate
{
    public enum UniSpyErrorCode : uint
    {
        // General.
        ///////////
        General = 0x0000,
        Parse,
        NotLoggedIn,
        BadSessionKey,
        DatabaseError,
        Network,
        ForcedDisconnect,
        ConnectionClose,
        UdpLayer,
    }
}