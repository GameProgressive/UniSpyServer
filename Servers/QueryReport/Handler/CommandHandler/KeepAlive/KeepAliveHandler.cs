using GameSpyLib.Common.Entity.Interface;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler : QRCommandHandlerBase
    {
        public KeepAliveHandler(IClient client, byte[] recv) : base(client, recv)
        {
        }

        protected override void ConstructeResponse()
        {
            KeepAlivePacket packet = new KeepAlivePacket();
            packet.Parse(_recv);
            _sendingBuffer = packet.GenerateResponse();
            QRClient client = (QRClient)_client.GetInstance();
            var result = GameServer.GetServers(client.RemoteEndPoint);
            if (result.Count != 1)
            {
                _errorCode = Entity.Enumerator.QRErrorCode.Database;
                return;
            }

            GameServer gameServer = result.First();

            gameServer.LastPacket = DateTime.Now;

            GameServer.UpdateServer
                (
                client.RemoteEndPoint,
                gameServer.ServerData.KeyValue["gamename"],
                gameServer);
        }
    }
}
