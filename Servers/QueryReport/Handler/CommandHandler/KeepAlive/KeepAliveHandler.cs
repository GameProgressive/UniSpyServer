using GameSpyLib.Logging;
using QueryReport.Entity.Structure;
using System;
using System.Net;
using QueryReport.Server;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler
    {
        public static void KeepAliveResponse(QRServer server, EndPoint endPoint, byte[] buffer)
        {
            byte[] sendingBuffer = new byte[7];
            sendingBuffer[0] = QR.QRMagic1;
            sendingBuffer[1] = QR.QRMagic2;
            sendingBuffer[2] = QRClient.KeepAlive;
            //According to SDK we know the instant key is from packet.BytesRecieved[1] to packet.BytesRecieved[4]
            //So we add it to response
            Array.Copy(buffer, 1, sendingBuffer, 3, 4);
            server.Send(endPoint, sendingBuffer);
            //We should keep the dedicated server in our server list
            //TODO
            LogWriter.Log.Write("[QR] Not finish function for KeepAlivePacket!", LogLevel.Debug);
        }
    }
}
