namespace StatsTracking.Entity.Enumerate
{
    /// <summary>
    /// In gamespy protocol there are no error response message
    /// from server to client, which mean we only need to make
    /// internal error system.
    /// </summary>
    public enum STError
    {
        General,
        Parse,
        Database,
        NoError
    }
}
