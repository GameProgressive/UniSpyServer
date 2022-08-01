using System.Linq;
using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;

using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Exception;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Response;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    
    public sealed class ChallengeHandler : CmdHandlerBase
    {
        private new ChallengeRequest _request => (ChallengeRequest)base._request;
        //we do not need to implement this to check the correctness of the challenge response
        private new ChallengeResult _result { get => (ChallengeResult)base._result; set => base._result = value; }
        public ChallengeHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new ChallengeResult();
        }

        protected override void DataOperation()
        {
            var servers = _redisClient.Context.Where(x => x.InstantKey == _request.InstantKey).ToList();
            if (servers.Count() == 0)
            {
                throw new QRException("No server found in redis, please make sure there is only one server.");
            }
            // _gameServerInfo = servers.First();
        }

        protected override void ResponseConstruct()
        {
            // We send the echo packet to check the ping
            _response = new ChallengeResponse(_request, _result);
        }
    }
}
