using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure;
using QueryReport.Network;
using Serilog.Events;
using System;
using System.Linq;
namespace QueryReport.Handler.CommandHandler
{
    public class EchoHandler : QRCommandHandlerBase
    {
        protected GameServer _gameServer;
        public EchoHandler(ISession session, IRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            //TODO
            var result = GameServer.GetServers(_session.RemoteEndPoint);
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
               _session.RemoteEndPoint,
               _gameServer.ServerData.KeyValue["gamename"],
               _gameServer
           );
        }
    }
}
