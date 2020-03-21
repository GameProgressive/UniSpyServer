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

        public int PublicIP { get; protected set; }
        public ushort PublicPort { get; protected set; }

        /// <summary>
        /// Instant key used to verity the client
        /// </summary>
        public byte[] InstantKey;

        public bool IsValidated = false;


        public ServerData ServerData { get; set; }
        public PlayerData PlayerData { get; set; }
        public TeamData TeamData { get; set; }

        public GameServer(EndPoint endPoint, byte[] instantKey)
        {
            InstantKey = new byte[4];
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
            PublicIP = BitConverter.ToInt32(((IPEndPoint)endPoint).Address.GetAddressBytes());
            PublicPort = (ushort)((IPEndPoint)endPoint).Port;
            instantKey.CopyTo(InstantKey, 0);
        }
    }
}
