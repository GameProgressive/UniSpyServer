using UniSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Enumerate;
using QueryReport.Network;
using System;
using System.Linq;

namespace QueryReport.Handler.CmdHandler
{
    public class KeepAliveHandler : QRCmdHandlerBase
    {
        protected new KeepAliveRequest _request { get { return (KeepAliveRequest)base._request; } }
        public KeepAliveHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void ConstructResponse()
        {
            if (_session.InstantKey != _request.InstantKey)
            {
                _session.InstantKey = _request.InstantKey;
            }

            _sendingBuffer = new QRResponseBase(_request).BuildResponse();
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_session.RemoteIPEndPoint);
            var result = GameServerInfo.RedisOperator.GetMatchedKeyValues(searchKey);
            if (result.Count != 1)
            {
                _errorCode = QRErrorCode.Database;
                return;
            }

            var gameServer = result.First();

            gameServer.Value.LastPacket = DateTime.Now;
            
            GameServerInfo.RedisOperator.SetKeyValue(gameServer.Key,gameServer.Value);
        }
    }
}
