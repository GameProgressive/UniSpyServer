using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using QueryReport.Entity.Structure;
using QueryReport.Server;
using Serilog.Events;
using System;
using System.Linq;
namespace QueryReport.Handler.CommandHandler.Echo
{
    public class EchoHandler : QRCommandHandlerBase
    {
        GameServer _gameServer;
        public EchoHandler(IClient client, byte[] recv) : base(client, recv)
        {
        }

        protected override void DataOperation()
        {
            //TODO
            QRClient client = (QRClient)_client.GetInstance();
            var result =
                 GameServer.GetServers(client.RemoteEndPoint);
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
               client.RemoteEndPoint,
               _gameServer.ServerData.KeyValue["gamename"],
               _gameServer
           );
        }
    }
}
