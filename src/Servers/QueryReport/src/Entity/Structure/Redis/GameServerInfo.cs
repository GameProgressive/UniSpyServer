using Newtonsoft.Json;
using UniSpyServer.Servers.QueryReport.Entity.Structure.ReportData;
using System;
using System.Net;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis
{
    /// <summary>
    /// This is the server 
    /// </summary>
    public class GameServerInfo
    {
        /// <summary>
        /// Last valid heart beat packet time
        /// </summary>
        public DateTime LastPacketReceivedTime { get; set; }
        public IPEndPoint RemoteQueryReportIPEndPoint { get; set; }
        [JsonIgnore]
        public string RemoteQueryReportIP => RemoteQueryReportIPEndPoint.Address.ToString();
        [JsonIgnore]
        public string RemoteQueryReportPort => RemoteQueryReportIPEndPoint.Port.ToString();
        public int InstantKey { get; set; }
        public ServerData ServerData { get; set; }
        public PlayerData PlayerData { get; set; }
        public TeamData TeamData { get; set; }

        public GameServerInfo()
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
            LastPacketReceivedTime = DateTime.Now;
        }

        public GameServerInfo(IPEndPoint endPoint)
        {
            ServerData = new ServerData();
            PlayerData = new PlayerData();
            TeamData = new TeamData();
            RemoteQueryReportIPEndPoint = endPoint;
            LastPacketReceivedTime = DateTime.Now;
        }
    }
}
