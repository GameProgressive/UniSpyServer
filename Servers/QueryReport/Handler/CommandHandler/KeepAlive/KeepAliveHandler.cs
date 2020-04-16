using System;
using System.Linq;
using GameSpyLib.Common.Entity.Interface;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler : QRCommandHandlerBase
    {
        public KeepAliveHandler(IClient client,byte[] recv) : base(client,recv)
        {
        }

        protected override void ConstructeResponse()
        {
            KeepAlivePacket packet = new KeepAlivePacket();
            packet.Parse(_recv);
            _sendingBuffer = packet.GenerateResponse();
            QRClient client = (QRClient)_client.GetInstance();
            var gameServer = GameServer.GetServers(client.RemoteEndPoint).First();
            gameServer.LastPacket = DateTime.Now;
            GameServer.UpdateServer
                (
                client.RemoteEndPoint,
                gameServer.ServerData.KeyValue["gamename"],
                gameServer);
        }
    }
}
