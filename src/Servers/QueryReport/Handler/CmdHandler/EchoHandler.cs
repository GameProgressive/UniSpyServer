using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure;
using Serilog.Events;
using System;
using System.Linq;
namespace QueryReport.Handler.CmdHandler
{
    public class EchoHandler : QRCmdHandlerBase
    {
        protected GameServerInfo _gameServer;
        public EchoHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            //TODO prevent one pc create multiple game servers
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_session.RemoteIPEndPoint);
            var matchedKeys = GameServerInfo.RedisOperator.GetMatchedKeys(searchKey);
            if (matchedKeys.Count() != 1)
            {
                LogWriter.ToLog(LogEventLevel.Error, "Can not find game server");
                return;
            }
            //add recive echo packet on gameserverList

            //we get the first key in matchedKeys
            _gameServer = GameServerInfo.RedisOperator.GetSpecificValue(matchedKeys[0]);

            _gameServer.LastPacket = DateTime.Now;

            GameServerInfo.RedisOperator.SetKeyValue(matchedKeys[0], _gameServer);
        }
    }
}
