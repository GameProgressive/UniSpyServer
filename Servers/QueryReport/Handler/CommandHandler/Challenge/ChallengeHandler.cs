using GameSpyLib.Logging;
using QueryReport.Entity.Structure;
using QueryReport.Server;
using System;
using System.Net;
namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler:QRHandlerBase
    {
        protected ChallengeHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            _sendingBuffer = new byte[7];
            _sendingBuffer[0] = QR.QRMagic1;
            _sendingBuffer[1] = QR.QRMagic2;
            _sendingBuffer[2] = QRGameServer.ClientRegistered;
            Array.Copy(recv, 1, _sendingBuffer, 3, 4);
        }
    }
}
