using System;
using System.Net;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This class contains the information about remote client
    /// </summary>
    public class ClientInfo
    {
        public DateTime LastKeepAliveTime;
        public DateTime LastHeartBeatTime;
        public DateTime LastPacketTime;
        public EndPoint RemoteEndPoint;
        public GameServerInfo ServerInfo;

        public ClientInfo(EndPoint remoteEndPoint, DateTime lastPacketTime)
        {
            RemoteEndPoint = remoteEndPoint;
            LastPacketTime = lastPacketTime;
        }
    }
}
