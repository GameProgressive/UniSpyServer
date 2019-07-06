using System;
using System.Net;

namespace RetroSpyServer.Servers.MasterServer.GameServerInfo
{
    internal class NonFilterAttribute : Attribute
    {
    }

    public class GameServer
    {
        [NonFilter]
        public IPEndPoint AddressInfo { get; protected set; }

        [NonFilter]
        public bool IsValidated = false;

        [NonFilter]
        public int QueryPort
        {
            get { return AddressInfo.Port; }
        }

        [NonFilter]
        public DateTime LastRefreshed { get; set; }

        [NonFilter]
        public DateTime LastPing { get; set; }

        [NonFilter]
        public string localip0 { get; set; }

        [NonFilter]
        public string localip1 { get; set; }

        [NonFilter]
        public int localport { get; set; }

        [NonFilter]
        public bool natneg { get; set; }

        [NonFilter]
        public int statechanged { get; set; }

        #region Server Vars

        public string country { get; set; }
        public string hostname { get; set; }
        public string gamename { get; set; }
        public string gamever { get; set; }
        public string mapname { get; set; }
        public string gametype { get; set; }
        public string gamevariant { get; set; }
        public int numplayers { get; set; }
        public int maxplayers { get; set; }
        public string gamemode { get; set; }
        public bool password { get; set; }
        public int timelimit { get; set; }
        public int roundtime { get; set; }
        public int hostport { get; set; }
        public bool game_dedicated { get; set; }
        public bool game_ranked { get; set; }
        public bool game_anticheat { get; set; }
        public string game_os { get; set; }
        public bool game_autorec { get; set; }
        public string game_d_idx { get; set; }
        public string game_d_dl { get; set; }
        public bool game_voip { get; set; }
        public bool game_autobalanced { get; set; }
        public bool game_friendlyfire { get; set; }
        public string game_tkmode { get; set; }
        public double game_startdelay { get; set; }
        public double game_spawntime { get; set; }
        public string game_sponsortext { get; set; }
        public string game_sponsorlogo_url { get; set; }
        public string game_communitylogo_url { get; set; }
        public int game_scorelimit { get; set; }
        public double game_ticketratio { get; set; }
        public double game_teamratio { get; set; }
        public string game_team1 { get; set; }
        public string game_team2 { get; set; }
        public bool game_bots { get; set; }
        public bool game_pure { get; set; }
        public int game_mapsize { get; set; }
        public bool game_globalunlocks { get; set; }
        public double game_fps { get; set; }
        public bool game_plasma { get; set; }
        public int game_reservedslots { get; set; }
        public double game_coopbotratio { get; set; }
        public int game_coopbotcount { get; set; }
        public int game_coopbotdiff { get; set; }
        public bool game_novehicles { get; set; }

        #endregion

        public GameServer(IPEndPoint addressInfo)
        {
            AddressInfo = addressInfo;
        }
    }
}
