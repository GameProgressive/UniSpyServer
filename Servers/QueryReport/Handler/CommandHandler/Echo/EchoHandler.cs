using GameSpyLib.Extensions;
using GameSpyLib.Logging;
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
                 GameServer.GetServers(endPoint);
            //add recive echo packet on gameserverList
            //DedicatedGameServer game;
            //QRServer.GameServerList.TryGetValue(endPoint, out game);

            if (result == null || result.Count() != 1)
            {
                LogWriter.ToLog(LogEventLevel.Error, "Can not find game server");
                return;
            }

            _gameServer = result.FirstOrDefault();

            _gameServer.LastPacket = DateTime.Now;
            GameServer.UpdateServer(
               endPoint,
               _gameServer.ServerData.KeyValue["gamename"],
               _gameServer
           );
        }
    }
}
