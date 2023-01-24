using System;
using System.Linq;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.V2.Application;
using UniSpyServer.Servers.QueryReport.V2.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Response;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.V2.Handler.CmdHandler
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

            // if (_request.PlayerData?.Count == 0 || _request.PlayerData is null)
            // {
            //     _client.LogInfo("Ignore incorrect implementation of heartbeat");
            //     return;
            // }
            CheckSpamGameServer();

            switch (_request.ReportType)
            {
                case HeartBeatReportType.ServerTeamPlayerData:
                    //normal heart beat
                    _gameServerInfo.ServerData = _request.ServerData;
                    _gameServerInfo.PlayerData = _request.PlayerData;
                    _gameServerInfo.TeamData = _request.TeamData;
                    break;
                case HeartBeatReportType.ServerPlayerData:
                    _gameServerInfo.ServerData = _request.ServerData;
                    _gameServerInfo.PlayerData = _request.PlayerData;
                    _gameServerInfo.LastPacketReceivedTime = DateTime.Now;
                    break;
                case HeartBeatReportType.ServerData:
                    _gameServerInfo.ServerData = _request.ServerData;
                    _gameServerInfo.LastPacketReceivedTime = DateTime.Now;
                    break;
            }
            UpdateGameServerByState();
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
                ServerID = _client.Connection.Server.ServerID,
                HostIPAddress = _client.Connection.RemoteIPEndPoint.Address,
                QueryReportPort = (ushort)_client.Connection.RemoteIPEndPoint.Port,
                GameName = _request.GameName,
                InstantKey = _request.InstantKey,
                ServerStatus = GameServerStatus.Normal,
                LastPacketReceivedTime = DateTime.Now
            };
        }
    }
}
