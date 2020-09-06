using System;
namespace QueryReport.Entity.Enumerator
{
    public enum HeartBeatReportType
    {
        ServerTeamPlayerData,
        ServerPlayerData,
        ServerData,
    }

    public enum GameServerServerStatus
    {
        Normal=0,
        Update=1,
        Shutdown=2,
        Playing = 3,
    }
}
