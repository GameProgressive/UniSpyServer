using System;
using System.Linq;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Redis;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdHandler
{
    internal sealed class KeepAliveHandler : QRCmdHandlerBase
    {
        private new QRDefaultRequest _request => (QRDefaultRequest)base._request;
        private new QRDefaultResult _result
        {
            get => (QRDefaultResult)base._result;
            set => base._result = value;
        }
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
                _result.ErrorCode = QRErrorCode.Database;
                return;
            }

            var gameServer = result.First();

            gameServer.Value.LastPacketReceivedTime = DateTime.Now;

            GameServerInfoRedisOperator.SetKeyValue(gameServer.Key, gameServer.Value);
        }
    }
}
