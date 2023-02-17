namespace UniSpy.Server.Core.Entity.Enumerate
{
    public enum UniSpyErrorCode : int
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