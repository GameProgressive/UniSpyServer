namespace UniSpyServer.Servers.GameStatus.Entity.Enumerate
{
    /// <summary>
    /// In gamespy protocol there are no error response message
    /// from server to client, which mean we only need to make
    /// public error system.
    /// </summary>
    public enum GSErrorCode
    {
        General,
        Parse,
        Database,
        NoError
    }
}
