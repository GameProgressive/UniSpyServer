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
        public byte[] PublicIP { get; protected set; }
        public byte[] PublicPort { get; protected set; }

        /// <summary>
        /// Instant key used to verity the client
        /// </summary>
        public byte[] InstantKey;

        public bool IsValidated = false;


        public ServerData Server { get; set; }
        public PlayerData Player { get; set; }
        public TeamData Team { get; set; }

        public GameServer(EndPoint endPoint, byte[] instantKey)
        {
            InstantKey = new byte[4];
            Server = new ServerData();
            Player = new PlayerData();
            Team = new TeamData();
            PublicIP = ((IPEndPoint)endPoint).Address.GetAddressBytes();
            PublicPort = BitConverter.GetBytes((ushort)((IPEndPoint)endPoint).Port);
            instantKey.CopyTo(InstantKey, 0);
        }





    }
}
