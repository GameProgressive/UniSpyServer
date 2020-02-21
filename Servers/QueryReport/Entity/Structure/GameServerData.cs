using System;
using System.Net;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This is How BattleSpy implement the Server information
    /// </summary>

    public class GameServerData
    {
        public GameServerData(EndPoint endPoint)
        {
            RemoteEndPoint = endPoint;
        }

        public int DatabaseId { get; set; }


        public EndPoint RemoteEndPoint { get; protected set; }


        public bool IsValidated = false;

        public int RemotePort
        {
            get { return ((IPEndPoint)RemoteEndPoint).Port; }
        }
        public byte[] RemoteIP
        {
            get { return ((IPEndPoint)RemoteEndPoint).Address.GetAddressBytes(); }
        }

        public DateTime LastRefreshed { get; set; }


        public DateTime LastPing { get; set; }


        public string LocalIp0 { get; set; }


        public string localIp1 { get; set; }


        public int LocalPort { get; set; }


        public bool NatNeg { get; set; }


        public int StateChanged { get; set; }

        #region Server Vars
        public string Country { get; set; }
        public string HostName { get; set; }
        public string GameName { get; set; }
        public string Gamever { get; set; }
        public string MapName { get; set; }
        public string GameType { get; set; }
        public string GameVariant { get; set; }
        public int NumPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public string GameMode { get; set; }
        public bool Password { get; set; }
        public int TimeLimit { get; set; }
        public int RoundTime { get; set; }
        public int HostPort { get; set; }
        public bool GameDedicated { get; set; }
        public bool GameRanked { get; set; }
        public bool GameAnticheat { get; set; }
        public string GameOS { get; set; }
        public bool GameAutorec { get; set; }
        public string GameDIdx { get; set; }
        public string GameDDl { get; set; }
        public bool GameVoip { get; set; }
        public bool GameAutoBalanced { get; set; }
        public bool GameFriendlyFire { get; set; }
        public string GameTkMode { get; set; }
        public double GameStartDelay { get; set; }
        public double GameSpawnTime { get; set; }
        public string GameSponsorText { get; set; }
        public string GameSponsorLogoUrl { get; set; }
        public string GameCommunityLogoUrl { get; set; }
        public int GameScoreLimit { get; set; }
        public double GameTicketRatio { get; set; }
        public double GameTeamRatio { get; set; }
        public string GameTeam1 { get; set; }
        public string GameTeam2 { get; set; }
        public bool GameBots { get; set; }
        public bool GamePure { get; set; }
        public int GameMapsize { get; set; }
        public bool GameGlobalUnlocks { get; set; }
        public double GameFps { get; set; }
        public bool GamePlasma { get; set; }
        public int GameReservedslots { get; set; }
        public double GameCoopBotRatio { get; set; }
        public int GameCoopBotCount { get; set; }
        public int GameCoopBotDiff { get; set; }
        public bool GameNoVehicles { get; set; }

        #endregion


    }
}
