using GameSpyLib.Abstraction.Interface;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Enumerate;
using QueryReport.Server;
using System;
using System.Linq;

namespace QueryReport.Handler.CommandHandler.KeepAlive
{
    public class KeepAliveHandler : QRCommandHandlerBase
    {
        private KeepAliveRequest _request;
        public KeepAliveHandler(ISession session, byte[] rawRequest) : base(session, rawRequest)
        {
            _request = new KeepAliveRequest(rawRequest);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }
        }

        protected override void ConstructeResponse()
        {
            QRResponseBase response = new QRResponseBase(_request);
            if (_session.InstantKey != _request.InstantKey)
            {
                _session.SetInstantKey(_request.InstantKey);
            }

            _sendingBuffer = response.BuildResponse();

            QRSession client = (QRSession)_session.GetInstance();
            var result = GameServer.GetServers(client.RemoteEndPoint);
            if (result.Count != 1)
            {
                _errorCode = QRErrorCode.Database;
                return;
            }

            GameServer gameServer = result.First();

            gameServer.LastPacket = DateTime.Now;

            GameServer.UpdateServer
                (
                client.RemoteEndPoint,
                gameServer.ServerData.KeyValue["gamename"],
                gameServer);
        }
    }
}
