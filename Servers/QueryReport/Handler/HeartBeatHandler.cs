using GameSpyLib.Logging;
using GameSpyLib.Network;

namespace QueryReport.Handler
{
    public class HeartBeatHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="qRServer"></param>
        /// <param name="packet"></param>
        public static void HeartbeatResponse(QRServer qRServer, UdpPacket packet)
        {
            LogWriter.Log.Write("[QR] No impliment function for Heartbeatpacket!", LogLevel.Debug);
            //TODO
        }
    }
}
