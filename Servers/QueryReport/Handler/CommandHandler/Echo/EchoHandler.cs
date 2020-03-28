using GameSpyLib.Extensions;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Net;
using System.Linq;
namespace QueryReport.Handler.CommandHandler.Echo
{
    public class EchoHandler : CommandHandlerBase
    {
        DedicatedGameServer _gameServer;
        public EchoHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
           var result =
                RetroSpyRedisExtensions.GetDedicatedGameServers<DedicatedGameServer>(endPoint);
            //add recive echo packet on gameserverList
            //DedicatedGameServer game;
            //QRServer.GameServerList.TryGetValue(endPoint, out game);

            if (result == null||result.Count()!=1)
            {
                server.ToLog("Can not find game server");
                return;
            }

            _gameServer = result.First();
            //compute the ping           
            byte ping = (byte)DateTime.Now.Subtract(_gameServer.LastPing).TotalMilliseconds;

            //adding ping and value to dictionary
            _gameServer.ServerData.StandardKeyValue.Add("ping", Convert.ToString(ping));

            RetroSpyRedisExtensions.UpdateDedicatedGameServer(
               endPoint,
               _gameServer.ServerData.StandardKeyValue["gamename"],
               _gameServer
           );
        }
    }
}
