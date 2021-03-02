using System.Linq;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Redis;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdHandler
{
    internal sealed class ChallengeHandler : QRCmdHandlerBase
    {
        private GameServerInfo _gameServerInfo;
        private new ChallengeRequest _request => (ChallengeRequest)base._request;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new ChallengeResult();
            if (_session.InstantKey != _request.InstantKey)
            {
                _result.ErrorCode = QRErrorCode.Parse;
            }
        }
        protected override void DataOperation()
        {
            var searchKey = new GameServerInfoRedisKey()
            {
                RemoteIPEndPoint = _session.RemoteIPEndPoint
            };

            var matchedKey = GameServerInfoRedisOperator.GetMatchedKeys(searchKey);
            if (matchedKey.Count() != 1)
            {
                _result.ErrorCode = QRErrorCode.Database;
                return;
            }
            var fullKey = matchedKey[0];
            _gameServerInfo = GameServerInfoRedisOperator.GetSpecificValue(fullKey);

            GameServerInfoRedisOperator.SetKeyValue(fullKey, _gameServerInfo);
        }

        protected override void ResponseConstruct()
        {
            // We send the echo packet to check the ping
            _response = new ChallengeResponse(_request, _result);
        }
    }
}
