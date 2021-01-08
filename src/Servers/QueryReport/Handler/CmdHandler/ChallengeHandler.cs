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
        protected GameServerInfo _gameServer;
        protected new ChallengeRequest _request { get { return (ChallengeRequest)base._request; } }
        protected string _fullKey;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            var searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_session.RemoteIPEndPoint);
            if (searchKey.Count() != 1)
            {
                _errorCode = QRErrorCode.Database;
                return;
            }
            _gameServer = GameServerInfo.RedisOperator.GetSpecificValue(searchKey);
        }

        protected override void ResponseConstruct()
        {
            if (_session.InstantKey != _request.InstantKey)
            {
                _session.InstantKey = _request.InstantKey;
            }

            // We send the echo packet to check the ping
            _sendingBuffer = new ChallengeResponse(_request).BuildResponse();
            GameServerInfo.RedisOperator.SetKeyValue(_fullKey, _gameServer);
        }


    }
}
