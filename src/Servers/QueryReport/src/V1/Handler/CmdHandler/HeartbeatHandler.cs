using System.Net;
using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.V1.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;
using UniSpy.Server.QueryReport.V1.Application;
using UniSpy.Server.QueryReport.V1.Contract.Request;
using UniSpy.Server.QueryReport.V1.Contract.Response;

namespace UniSpy.Server.QueryReport.V1.Handler.CmdHandler
{
    public sealed class HeartbeatHandler : CmdHandlerBase
    {
        
        private new HeartbeatRequest _request => (HeartbeatRequest)base._request;
        public HeartbeatHandler(Client client, HeartbeatRequest request) : base(client, request)
        {
        }
        protected override void DataOperation()
        {
            // _result.Challenge = Challenge;
            var gameServerEnd = new IPEndPoint(_client.Connection.RemoteIPEndPoint.Address, _request.QueryReportPort);
            _client.Info.GameSecretKey = StorageOperation.Persistance.GetGameSecretKey(_request.GameName);
            // _result.Challenge = Challenge;
            var info = new GameServerInfo()
            {
                ServerID = _client.Server.Id,
                HostIPAddress = _client.Connection.RemoteIPEndPoint.Address,
                // check whether this indicate game port
                QueryReportPort = _request.QueryReportPort,
                GameName = _request.GameName,
                KeyValues = _request.KeyValues
            };
            //todo whether we need to update the part that info changed
            StorageOperation.Persistance.UpdateServerInfo(info);
        }
        protected override void ResponseConstruct()
        {
            _response = new HeartbeatResponse(_request);
        }
    }
}