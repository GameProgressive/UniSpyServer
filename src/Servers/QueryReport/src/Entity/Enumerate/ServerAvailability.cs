namespace UniSpyServer.Servers.QueryReport.Entity.Enumerate
{
    public enum ServerAvailability : byte
    {
        Available = 0x00,
        PermanentUnavailable = 0x01,
        TemporarilyUnavailable = 0x02,
    };
}
