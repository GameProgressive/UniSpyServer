using System;
using System.Linq;
using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Response;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    [HandlerContract(RequestType.HeartBeat)]
    public sealed class HeartBeatHandler : CmdHandlerBase
    {
        private new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        private GameServerInfo _gameServerInfo;
        private new HeartBeatResult _result { get => (HeartBeatResult)base._result; set => base._result = value; }
        public HeartBeatHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new HeartBeatResult();
        }
        protected override void DataOperation()
        {
            //parse the endpoint information into result class
            _result.RemoteIPEndPoint = _client.Session.RemoteIPEndPoint;

            if (_request.PlayerData?.Count == 0 || _request.PlayerData == null)
            {
                LogWriter.Info("Ignore incorrect implementation of heartbeat");
                return;
            }
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
                _redisClient.DeleteKeyValue(_gameServerInfo);
            }
            else
            {
                _redisClient.SetValue(_gameServerInfo);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new HeartBeatResponse(_request, _result);
        }

        private void CheckSpamGameServer()
        {
            if (_request.PlayerData.Count == 0)
            {

            }
            //make sure one ip address create one server on each game
            //we check if the database have multiple game server if it contains
            _gameServerInfo = _redisClient.Values.FirstOrDefault(x =>
                                    x.ServerID == _client.Session.Server.ServerID &
                                    x.HostIPAddress == _client.Session.RemoteIPEndPoint.Address &
                                    x.QueryReportPort == _client.Session.RemoteIPEndPoint.Port &
                                    x.InstantKey == _request.InstantKey &
                                    x.GameName == _request.GameName);

            if (_gameServerInfo == null)
            {
                _gameServerInfo = new GameServerInfo()
                {
                    ServerID = _client.Session.Server.ServerID,
                    HostIPAddress = _client.Session.RemoteIPEndPoint.Address,
                    QueryReportPort = (ushort)_client.Session.RemoteIPEndPoint.Port,
                    GameName = _request.GameName,
                    InstantKey = _request.InstantKey,
                    ServerStatus = GameServerStatus.Normal,
                    LastPacketReceivedTime = DateTime.Now
                };
            }
        }
    }
}
