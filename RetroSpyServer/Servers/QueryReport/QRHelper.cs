using RetroSpyServer.Servers.QueryReport.GameServerInfo;
using System;
using RetroSpyServer.DBQueries;
using GameSpyLib.Network;
using GameSpyLib.Logging;
using GameSpyLib.Common;

namespace RetroSpyServer.Servers.QueryReport
{
    /// <summary>
    /// This class contians gamespy master udp server functions  which help cdkeyserver to finish the master tcp server functionality. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
   public class QRHelper
   {
        public static QRDBQuery DBQuery = null;

        public static void UpdateServerOffline(GameServer server)
        {
            DBQuery.UpdateServerOffline(server);
        }

        public static void HeartbeatResponse(QRServer qRServer, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }

        public static void EchoResponse(QRServer qRServer, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }

        public static void KeepAlive(QRServer qRServer, UdpPacket packet)
        {
            LogWriter.Log.Write("heartbeat response packets " + packet.BytesRecieved.ToString(), LogLevel.Debug);
        }
    }
}
