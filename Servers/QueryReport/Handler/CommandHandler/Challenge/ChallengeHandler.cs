using GameSpyLib.Logging;
using QueryReport.Entity.Structure;
using QueryReport.Server;
using System;
using System.Net;
namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler
    {
        public static void ServerChallengeResponse(QRServer server, EndPoint endPoint, byte[] buffer)
        {
            byte[] sendingbuffer = new byte[7];
            sendingbuffer[0] = QR.QRMagic1;
            sendingbuffer[1] = QR.QRMagic2;
            sendingbuffer[2] = QRGameServer.ClientRegistered;
            Array.Copy(buffer, 1, sendingbuffer, 3, 4);

            server.SendAsync(endPoint, sendingbuffer);
        }
    }
}
