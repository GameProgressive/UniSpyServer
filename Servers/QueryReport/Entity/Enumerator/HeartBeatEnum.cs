using System;
namespace QueryReport.Entity.Enumerator
{
    public enum HeartBeatReportType
    {
        ServerTeamPlayerData,
        ServerPlayerData,
        ServerData,
    }
    public enum HeartBeartServerState
    {
        Start=0,
        Running=1,
        Playing=2,
        Shutdown=3
    }
}
