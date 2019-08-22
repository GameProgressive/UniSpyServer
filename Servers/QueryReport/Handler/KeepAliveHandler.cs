using GameSpyLib.Logging;
using GameSpyLib.Network;
using QueryReport.Structures;
using System;

namespace QueryReport.Handler
{
    public class KeepAliveHandler
    {
        public static void KeepAliveResponse(QRServer server, UdpPacket packet)
        {
            byte[] sendingBuffer = new byte[7];
            sendingBuffer[0] = QR.QRMagic1;
            sendingBuffer[1] = QR.QRMagic2;
            sendingBuffer[2] = QRClientRequest.KeepAlive;
            //According to SDK we know the instant key is from packet.BytesRecieved[1] to packet.BytesRecieved[4]
            //So we add it to response
            Array.Copy(packet.BytesRecieved, 1, sendingBuffer, 3, 4);
            server.SendAsync(packet, sendingBuffer);
            //We should keep the dedicated server in our server list
            //TODO
            LogWriter.Log.Write("[QR] Not finish function for KeepAlivePacket!", LogLevel.Debug);
        }
    }
}
