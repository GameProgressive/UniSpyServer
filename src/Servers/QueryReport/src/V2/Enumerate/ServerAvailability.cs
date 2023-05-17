namespace UniSpy.Server.QueryReport.V2.Enumerate
{
    public enum ServerAvailability : uint
    {
        Available = 0,
        Waiting = 1,
        PermanentUnavailable = 2,
        TemporarilyUnavailable = 3,
    };
}
