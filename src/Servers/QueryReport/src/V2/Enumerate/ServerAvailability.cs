namespace UniSpy.Server.QueryReport.V2.Enumerate
{
    public enum ServerAvailability : byte
    {
        Waiting = 0x00,
        Available = 0x01,
        PermanentUnavailable = 0x02,
        TemporarilyUnavailable = 0x03,
    };
}
