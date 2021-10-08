namespace QueryReport.Entity.Enumerate
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
        Playing = 3,
    }
}
