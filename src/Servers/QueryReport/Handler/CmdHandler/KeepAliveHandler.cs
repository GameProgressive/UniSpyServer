using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using QueryReport.Handler.SystemHandler.Redis;
using System;
using System.Linq;
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
            if (_session.InstantKey != _request.InstantKey)
            {
                _session.InstantKey = _request.InstantKey;
            }
            _response = new QRDefaultResponse(_request, _result);
            var searchKey = GameServerInfoRedisOperator.BuildSearchKey(_session.RemoteIPEndPoint);
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
