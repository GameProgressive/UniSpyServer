using GameSpyLib.Extensions;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using Serilog.Events;
using System.Linq;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler : CommandHandlerBase
    {
        DedicatedGameServer _gameServer;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler() : base()
        {
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            var result =
                  RedisExtensions.GetDedicatedGameServers<DedicatedGameServer>(endPoint);

            if (result.Count() != 1)
            {
                server.ToLog(LogEventLevel.Error, "Can not find game server in game server list");
                return;
            }
            _gameServer = result.First();
            server.ToLog(LogEventLevel.Error, "Challenge received game server is now available");
            _gameServer.IsValidated = true;
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            EchoPacket echo = new EchoPacket();
            echo.Parse(recv);
            // We send the echo packet to check the ping
            _sendingBuffer = echo.GenerateResponse();

            RedisExtensions.UpdateDedicatedGameServer(
                endPoint,
                _gameServer.ServerData.StandardKeyValue["gamename"],
                _gameServer
                );

            //DedicatedGameServer gameServer;
            //QRServer.GameServerList.TryGetValue(endPoint, out gameServer);
        }
    }
}
