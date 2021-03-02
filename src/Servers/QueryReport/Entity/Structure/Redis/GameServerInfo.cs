using QueryReport.Entity.Structure.ReportData;
using System;
using System.Net;

namespace QueryReport.Entity.Structure.Redis
{
    /// <summary>
    /// This is the server 
    /// </summary>
    public class GameServerInfo
    {
        /// <summary>
        /// Last valid heart beat packet time
        /// </summary>
        public DateTime LastPacketReceivedTime;
        public IPEndPoint RemoteQueryReportIPEndPoint;
        public string RemoteQueryReportIP => RemoteQueryReportIPEndPoint.Address.ToString();
        public string RemoteQueryReportPort => RemoteQueryReportIPEndPoint.Port.ToString();
        public int InstantKey;
        public ServerData ServerData;
        public PlayerData PlayerData;
        public TeamData TeamData;

        public GameServerInfo()
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
            LastPacketReceivedTime = DateTime.Now;
        }

        public GameServerInfo(EndPoint endPoint)
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
            RemoteQueryReportIPEndPoint = (IPEndPoint)endPoint;
            LastPacketReceivedTime = DateTime.Now;
        }
    }
}
