using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Echo
{
    public class EchoHandler : CommandHandlerBase
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
            
            //compute the ping
           
            byte ping = (byte)DateTime.Now.Subtract(game.LastPing).TotalMilliseconds;

            //adding ping and value to dictionary
            game.ServerData.StandardKeyValue.Add("ping", Convert.ToString(ping));
        }
    }
}
