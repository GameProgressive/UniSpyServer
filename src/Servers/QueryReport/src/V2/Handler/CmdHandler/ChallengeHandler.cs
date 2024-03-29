using System.Linq;
using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Contract.Request;
using UniSpy.Server.QueryReport.V2.Contract.Response;
using UniSpy.Server.QueryReport.V2.Contract.Result;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class ChallengeHandler : CmdHandlerBase
    {
        private new ChallengeRequest _request => (ChallengeRequest)base._request;
        //we do not need to implement this to check the correctness of the challenge response
        private new ChallengeResult _result { get => (ChallengeResult)base._result; set => base._result = value; }
        public ChallengeHandler(Client client, ChallengeRequest request) : base(client, request)
        {
            _result = new ChallengeResult();
        }

        protected override void DataOperation()
        {
            var servers = QueryReport.V2.Application.StorageOperation.Persistance.GetServerInfos((uint)_request.InstantKey);
            if (servers.Count() == 0)
            {
                throw new QueryReport.Exception("No server found in redis, please make sure there is only one server.");
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
