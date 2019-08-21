using GameSpyLib.Logging;
using GameSpyLib.Network;

namespace QueryReport.Handler
{
    public class KeepAliveHandler
    {
        public static void KeepAliveResponse(QRServer server, UdpPacket packet)
        {
            LogWriter.Log.Write("[QR] No impliment function for KeepAlivePacket!", LogLevel.Debug);
            //TODO
        }
    }
}
