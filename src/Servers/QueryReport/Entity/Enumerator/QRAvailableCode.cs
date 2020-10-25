namespace QueryReport.Entity.Enumerator
{
    public enum ServerAvailability : byte
    {
        Available = 0x00,
        PermanentUnavailable = 0x01,
        TemporarilyUnavailable = 0x02,
    };
}
