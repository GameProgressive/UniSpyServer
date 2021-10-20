using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.QueryReport.Entity.Exception;
using UniSpyServer.QueryReport.Entity.Structure.Redis;
using UniSpyServer.QueryReport.Entity.Structure.Request;
using UniSpyServer.QueryReport.Entity.Structure.Response;
using UniSpyServer.QueryReport.Entity.Structure.Result;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.QueryReport.Handler.CmdHandler
{
    [HandlerContract(RequestType.Challenge)]
    public sealed class ChallengeHandler : CmdHandlerBase
    {
        private GameServerInfo _gameServerInfo;
        private new ChallengeRequest _request => (ChallengeRequest)base._request;
        //we do not need to implement this to check the correctness of the challenge response
        public ChallengeHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ChallengeResult();
        }

        protected override void DataOperation()
        {
            var searchKey = new GameServerInfoRedisKey()
            {
                InstantKey = _request.InstantKey
            };

            var matchedKey = GameServerInfoRedisOperator.GetMatchedKeys(searchKey);
            if (matchedKey.Count() != 1)
            {
                throw new QRException("No server or multiple servers found in redis, please make sure there is only one server.");
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
