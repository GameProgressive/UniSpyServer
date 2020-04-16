using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using Serilog.Events;
using System.Linq;
using System.Net;

namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler : QRCommandHandlerBase
    {
        GameServer _gameServer;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(IClient client, byte[] recv) : base(client,recv)
        {
        }

        protected override void DataOperation()
        {
            QRClient client = (QRClient)_client.GetInstance();
            var result =
                  GameServer.GetServers(client.RemoteEndPoint);

            if (result.Count() != 1)
            {
                _errorCode = Entity.Enumerator.QRErrorCode.Database;
                return;
            }
            _gameServer = result.First();
        }

        protected override void ConstructeResponse()
        {
            EchoPacket echo = new EchoPacket();
            echo.Parse(_recv);
            // We send the echo packet to check the ping
            _sendingBuffer = echo.GenerateResponse();

            QRClient client = (QRClient)_client.GetInstance();

            GameServer.UpdateServer(
                client.RemoteEndPoint,
                _gameServer.ServerData.KeyValue["gamename"],
                _gameServer
                );
        }
    }
}
