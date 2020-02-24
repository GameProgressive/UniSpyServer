namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerInfo
    {
        public string GameName;
        public byte[] HostName;
        public byte[] HostPort;
        public byte[] GameVersion;
        public byte[] MapName;
        public byte[] GameType;
        public byte[] GameVariant;
        public byte[] NumPlayers;
        public byte[] NumTeams;
        public byte[] MaxPlayers;
        public byte[] GameMode;
        public byte[] TeamPlay;
        public byte[] Fraglimit,
    TeamFragLimit,
    TimeLimit,
    TimeElapsed,
    RoundTime,
    RoundElapsed,
    Password,
    GroupID;
        public string LocalIp0 { get; set; }
        public string localIp1 { get; set; }
        public int LocalPort { get; set; }
        public bool NatNeg { get; set; }
        public int StateChanged { get; set; }
    }
}
