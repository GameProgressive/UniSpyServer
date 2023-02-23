using System.Net;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;
using UniSpy.Server.QueryReport.V1.Application;
using UniSpy.Server.QueryReport.V1.Contract.Request;
using UniSpy.Server.QueryReport.V1.Contract.Response;
using UniSpy.Server.QueryReport.V1.Contract.Result;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public sealed class HeartbeatHandler : CmdHandlerBase
    {
        /// <summary>
        /// Hard coded challenge, we are not gamespy we do not care about the challenge randomness
        /// </summary>
        public static string Challenge = "000000";
        private new HeartbeatRequest _request => (HeartbeatRequest)base._request;
        private new HeartbeatResult _result { get => (HeartbeatResult)base._result; set => base._result = value; }
        public HeartbeatHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new HeartbeatResult();
        }
        protected override void DataOperation()
        {
            _result.Challenge = Challenge;
            var gameServerEnd = new IPEndPoint(_client.Connection.RemoteIPEndPoint.Address, _request.QueryReportPort);
            _client.Info.GameSecretKey = StorageOperation.Persistance.GetGameSecretKey(_request.GameName);
            _result.Challenge = Challenge;
            var info = new GameServerInfo()
            {
                ServerID = _client.Connection.Server.ServerID,
                HostIPAddress = _client.Connection.RemoteIPEndPoint.Address,
                // check whether this indicate game port
                QueryReportPort = _request.QueryReportPort,
                GameName = _request.GameName,
                GameData = _request.RawRequest
            };
            //todo whether we need to update the part that info changed
            StorageOperation.Persistance.UpdateServerInfo(info);
        }
        protected override void ResponseConstruct()
        {
            _response = new HeartbeatResponse(_request, _result);
        }
    }
}