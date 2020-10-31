using GameSpyLib.Abstraction.Interface;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using QueryReport.Server;
using System.Linq;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Response;

namespace QueryReport.Handler.CommandHandler.Challenge
{
    public class ChallengeHandler : QRCommandHandlerBase
    {
        GameServer _gameServer;
        EchoRequest _request;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(ISession session, byte[] rawRequest) : base(session, rawRequest)
        {
            _request = new EchoRequest(rawRequest);
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

        protected override void DataOperation()
        {
            QRSession session = (QRSession)_session.GetInstance();
            var result =
                  GameServer.GetServers(session.RemoteEndPoint);

            if (result.Count() != 1)
            {
                _errorCode = QRErrorCode.Database;
                return;
            }
            _gameServer = result.First();
        }

        protected override void ConstructeResponse()
        {
            EchoResponse response = new EchoResponse(_request);

            if (_session.InstantKey != _request.InstantKey)
            {
                _session.SetInstantKey(_request.InstantKey);
            }

            // We send the echo packet to check the ping
            _sendingBuffer = response.BuildResponse();

            GameServer.UpdateServer(
                _session.RemoteEndPoint,
                _gameServer.ServerData.KeyValue["gamename"],
                _gameServer
                );
        }


    }
}
