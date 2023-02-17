namespace UniSpy.Server.PresenceConnectionManager.Entity.Enumerate
{
    public enum QuietModeType : uint
    {
        // Quiet mode flags.
        ////////////////////
        SlienceNone = 0x00000000,
        SlienceMessage = 0x00000001,
        SlienceUTMS = 0x00000002,
        SlienceList = 0x00000004, // includes requests, auths, and revokes
        SlienceAll = 0xFFFFFFFF,
    }
}
