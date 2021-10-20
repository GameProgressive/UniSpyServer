using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.QueryReport.Entity.Exception;
using UniSpyServer.QueryReport.Entity.Structure.Redis;
using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.QueryReport.Handler.CmdHandler
{
    [HandlerContract(RequestType.KeepAlive)]
    public sealed class KeepAliveHandler : CmdHandlerBase
    {
        public KeepAliveHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void DataOperation()
        {
            var searchKey = new GameServerInfoRedisKey()
            {
                InstantKey = _request.InstantKey
            };

            var result = GameServerInfoRedisOperator.GetMatchedKeyValues(searchKey);
            if (result.Count != 1)
            {
                throw new QRException("No server or multiple servers found in redis, please make sure there is only one server.");
            }

            var gameServer = result.First();

            gameServer.Value.LastPacketReceivedTime = DateTime.Now;

            GameServerInfoRedisOperator.SetKeyValue(gameServer.Key, gameServer.Value);
        }
    }
}
