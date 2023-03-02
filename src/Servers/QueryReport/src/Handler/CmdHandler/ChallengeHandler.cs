using System.Linq;
using UniSpy.Server.QueryReport.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.Exception;
using UniSpy.Server.QueryReport.Contract.Request;
using UniSpy.Server.QueryReport.Contract.Response;
using UniSpy.Server.QueryReport.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.Handler.CmdHandler
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
            var servers = StorageOperation.Persistance.GetServerInfos((uint)_request.InstantKey);
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
