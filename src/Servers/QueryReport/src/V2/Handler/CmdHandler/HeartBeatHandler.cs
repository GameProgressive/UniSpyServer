using System;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Application;
using UniSpy.Server.QueryReport.V2.Enumerate;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.QueryReport.V2.Contract.Request;
using UniSpy.Server.QueryReport.V2.Contract.Response;
using UniSpy.Server.QueryReport.V2.Contract.Result;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.V2.Handler.CmdHandler
{

    public sealed class HeartBeatHandler : CmdHandlerBase
    {
        private new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        private new HeartBeatResult _result { get => (HeartBeatResult)base._result; set => base._result = value; }
        private GameServerInfo _gameServerInfo;
        public HeartBeatHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new HeartBeatResult();
        }
        protected override void DataOperation()
        {
            //Parse the endpoint information into result class
            _result.RemoteIPEndPoint = _client.Connection.RemoteIPEndPoint;

            CheckSpamGameServer();
            // todo check if server data is null
            //normal heart beat
            _gameServerInfo.ServerData = _request.ServerData;
            _gameServerInfo.PlayerData = _request.PlayerData;
            _gameServerInfo.TeamData = _request.TeamData;
            _gameServerInfo.LastPacketReceivedTime = DateTime.Now;
            UpdateGameServerByState();
            // we publish message to notice all sb server to send adhoc message to their clients.
            StorageOperation.HeartbeatChannel.PublishMessage(_gameServerInfo);
        }

        private void UpdateGameServerByState()
        {
            if (_gameServerInfo.ServerStatus == GameServerStatus.Shutdown)
            {
                StorageOperation.Persistance.RemoveGameServer(_gameServerInfo);
            }
            else
            {
                StorageOperation.Persistance.UpdateGameServer(_gameServerInfo);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new HeartBeatResponse(_request, _result);
        }

        private void CheckSpamGameServer()
        {
            // Ensures that an IP address creates a server for each game, we check if redis has multiple game servers
            _gameServerInfo = new GameServerInfo()
            {
                ServerID = _client.Server.Id,
                HostIPAddress = _client.Connection.RemoteIPEndPoint.Address,
                QueryReportPort = (ushort)_client.Connection.RemoteIPEndPoint.Port,
                GameName = _request.GameName,
                InstantKey = _request.InstantKey,
                ServerStatus = _request.ServerStatus,
                LastPacketReceivedTime = DateTime.Now
            };
        }
    }
}
