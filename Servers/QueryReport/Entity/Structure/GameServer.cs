using System;
using System.Net;
using QueryReport.Entity.Structure.ReportData;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This is the server 
    /// </summary>

    public class GameServer
    {
        /// <summary>
        /// Last valid heart beat packet time
        /// </summary>
        public DateTime LastHeartBeatPacket { get; set; }

        /// <summary>
        /// Last keep alive packet time
        /// </summary>
        public DateTime LastKeepAlive { get; set; }

        /// <summary>
        /// Last ping packet time
        /// </summary>
        public DateTime LastPing { get; set; }

        public int RemoteIP { get; set; }
        public ushort RemotePort { get; set; }

        /// <summary>
        /// Instant key used to verity the client
        /// </summary>
        public int InstantKey;

        public bool IsValidated = false;


        public ServerData ServerData { get; set; }
        public PlayerData PlayerData { get; set; }
        public TeamData TeamData { get; set; }

        public GameServer()
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
        }

        public void Parse(EndPoint endPoint, int instantKey)
        {
            RemoteIP = BitConverter.ToInt32(((IPEndPoint)endPoint).Address.GetAddressBytes());
            RemotePort = (ushort)((IPEndPoint)endPoint).Port;
            InstantKey = instantKey;
        }

    }
}
