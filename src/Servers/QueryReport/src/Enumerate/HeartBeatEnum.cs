namespace UniSpy.Server.QueryReport.Enumerate
{
    public enum HeartBeatReportType
    {
        ServerTeamPlayerData,
        ServerPlayerData,
        ServerData,
    }

    public enum GameServerStatus
    {
        Normal = 0,
        Update = 1,
        Shutdown = 2,
        // todo check this flag
        Playing = 3,
    }
}
