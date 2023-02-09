using System.Linq;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Application;
using UniSpyServer.Servers.QueryReport.V2.Entity.Exception;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
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
