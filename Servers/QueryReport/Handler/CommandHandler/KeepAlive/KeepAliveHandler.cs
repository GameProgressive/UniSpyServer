using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler : CommandHandlerBase
    {
        public KeepAliveHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            KeepAlivePacket packet = new KeepAlivePacket();
            packet.Parse(recv);
            _sendingBuffer = packet.GenerateResponse();
            GameServer game;
            QRServer.GameServerList.TryGetValue(endPoint, out game);
            game.LastKeepAlive = DateTime.Now;
        }
    }
}
