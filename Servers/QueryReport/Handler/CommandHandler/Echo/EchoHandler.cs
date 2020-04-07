using GameSpyLib.Extensions;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure;
using QueryReport.Server;
using Serilog.Events;
using System;
using System.Linq;
using System.Net;
namespace QueryReport.Handler.CommandHandler.Echo
{
    public class EchoHandler : CommandHandlerBase
    {
         GameServer _gameServer;
        public EchoHandler() : base()
        {
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            var result =
                 RedisExtensions.GetDedicatedGameServers<GameServer>(endPoint);
            //add recive echo packet on gameserverList
            //DedicatedGameServer game;
            //QRServer.GameServerList.TryGetValue(endPoint, out game);

            if (result == null || result.Count() != 1)
            {
                server.ToLog(LogEventLevel.Error, "Can not find game server");
                return;
            }

            _gameServer = result.First();
            //compute the ping           
            byte ping = (byte)DateTime.Now.Subtract(_gameServer.LastPing).TotalMilliseconds;

            //adding ping and value to dictionary
            _gameServer.ServerData.KeyValue.Add("ping", Convert.ToString(ping));

            RedisExtensions.UpdateDedicatedGameServer(
               endPoint,
               _gameServer.ServerData.KeyValue["gamename"],
               _gameServer
           );
        }
    }
}
