using GameSpyLib.Extensions;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Net;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler : CommandHandlerBase
    {
        public KeepAliveHandler() : base()
        {
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            KeepAlivePacket packet = new KeepAlivePacket();
            packet.Parse(recv);
            _sendingBuffer = packet.GenerateResponse();
            var gameServer = GameServer.GetGameServers(endPoint).First();
            gameServer.LastKeepAlive = DateTime.Now;
            GameServer.UpdateGameServer
                (
                endPoint,
                gameServer.ServerData.KeyValue["gamename"],
                gameServer);
        }
    }
}
