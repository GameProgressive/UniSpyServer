using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Echo
{
    public class EchoHandler : QRHandlerBase
    {
        public EchoHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            //add recive echo packet on gameserverList
            GameServer game;
            QRServer.GameServerList.TryGetValue(endPoint, out game);
            if (game == null)
            {
                server.ToLog("Can not find game server");
                return;
            }
            double ping = DateTime.Now.Subtract(game.LastPing).TotalSeconds;
            game.ServerKeyValue.Data.Add("ping", ping.ToString());
        }
    }
}
