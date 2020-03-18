using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler : CommandHandlerBase
    {
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            Entity.Structure.GameServer gameServer;
            QRServer.GameServerList.TryGetValue(endPoint, out gameServer);
            if (gameServer == null)
            {
                server.ToLog("Can not find game server in game server list");
                return;
            }
            server.ToLog("Challenge received game server is now available");
            gameServer.IsValidated = true;
        }
        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            EchoPacket echo = new EchoPacket(recv);
            // We send the echo packet to check the ping
            _sendingBuffer = echo.GenerateResponse();
            Entity.Structure.GameServer gameServer;
            QRServer.GameServerList.TryGetValue(endPoint, out gameServer);
            gameServer.LastPing = DateTime.Now;

        }
    }
}
