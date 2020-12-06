using UniSpyLib.Abstraction.Interface;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using System.Linq;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Response;

namespace QueryReport.Handler.CmdHandler
{
    public class ChallengeHandler : QRCmdHandlerBase
    {
        protected GameServer _gameServer;
        protected new ChallengeRequest _request { get { return (ChallengeRequest)base._request; } }
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            var result =
                  GameServer.GetServers(_session.RemoteEndPoint);

            if (result.Count() != 1)
            {
                _errorCode = QRErrorCode.Database;
                return;
            }
            _gameServer = result.First();
        }

        protected override void ConstructResponse()
        {
            if (_session.InstantKey != _request.InstantKey)
            {
                _session.SetInstantKey(_request.InstantKey);
            }

            // We send the echo packet to check the ping
            _sendingBuffer = new ChallengeResponse(_request).BuildResponse();

            GameServer.UpdateServer(
                _session.RemoteEndPoint,
                _gameServer.ServerData.KeyValue["gamename"],
                _gameServer
                );
        }


    }
}
