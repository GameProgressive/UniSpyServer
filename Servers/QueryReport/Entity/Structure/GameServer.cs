using QueryReport.Entity.Structure.ReportData;
using System;
using System.Net;

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
        public DateTime LastRefreshed { get; set; }
        /// <summary>
        /// Last keep alive packet time
        /// </summary>
        public DateTime LastKeepAlive { get; set; }
        /// <summary>
        /// Last ping packet time
        /// </summary>
        public DateTime LastPing { get; set; }

        public GameServer(EndPoint endPoint)
        {
            RemoteEndPoint = endPoint;
        }
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


        public ServerInfo ServerInfo = new ServerInfo();
        public PlayerInfo PlayerInfo = new PlayerInfo();
        public TeamInfo TeamInfo = new TeamInfo();

    }
}
