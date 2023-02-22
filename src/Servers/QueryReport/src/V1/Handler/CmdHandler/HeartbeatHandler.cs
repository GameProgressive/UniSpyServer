using System.Linq;
using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Contract.Request;
using UniSpy.Server.QueryReport.V1.Contract.Response;
using UniSpy.Server.QueryReport.V1.Contract.Result;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public class HeartbeatHandler : CmdHandlerBase
    {
        public static string Challenge = Enumerable.Repeat("0", 64).ToString();
        private new HeartbeatRequest _request => (HeartbeatRequest)base._request;
        private new HeartbeatResult _result { get => (HeartbeatResult)base._result; set => base._result = value; }
        public HeartbeatHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new HeartbeatResult();
        }
        protected override void DataOperation()
        {
            _result.Challenge = Challenge;
            var gameServerEnd = new IPEndPoint(_client.Connection.RemoteIPEndPoint.Address, (int)_request.QueryReportPort);
        }
        protected override void ResponseConstruct()
        {
            _response = new HeartbeatResponse(_request, _result);
        }
    }
}