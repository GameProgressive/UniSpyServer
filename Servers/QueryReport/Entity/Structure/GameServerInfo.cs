using System;
using System.Net;

namespace QueryReport.Entity.Structure
{
    /// <summary>
    /// This class contains the information about remote client
    /// </summary>
    public class GameServerInfo
    {
        public DateTime LastKeepAliveTime;
        public DateTime LastHeartBeatTime;
        public DateTime LastPacketTime;
        public EndPoint RemoteEndPoint;
        public GameServerData GameServerData;

        public GameServerInfo(EndPoint remoteEndPoint, DateTime lastPacketTime)
        {
            RemoteEndPoint = remoteEndPoint;
            LastPacketTime = lastPacketTime;
        }
    }
}
