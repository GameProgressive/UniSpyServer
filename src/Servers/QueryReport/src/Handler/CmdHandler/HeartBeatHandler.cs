using UniSpyServer.Servers.QueryReport.Abstraction.BaseClass;
using UniSpyServer.Servers.QueryReport.Application;
using UniSpyServer.Servers.QueryReport.Entity.contract;
using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Request;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Response;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Result;
using System;
using System.Linq;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;

namespace UniSpyServer.Servers.QueryReport.Handler.CmdHandler
{
    [HandlerContract(RequestType.HeartBeat)]
    public sealed class HeartBeatHandler : CmdHandlerBase
    {
        private new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        private GameServerInfo2 _gameServerInfo;
        private new HeartBeatResult _result { get => (HeartBeatResult)base._result; set => base._result = value; }
        public HeartBeatHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new HeartBeatResult();
        }

        protected override void DataOperation()
        {
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
            //parse the endpoint information into result class
            _result.RemoteIPEndPoint = _session.RemoteIPEndPoint;
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
            //make sure one ip address create one server on each game

            var searchKey = new GameServerInfoRedisKey()
            {
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                GameName = _request.GameName
            };

            var matchedKeys =
                GameServerInfoRedisOperator.GetMatchedKeys(searchKey);

            var duplicatedKeys = matchedKeys.Where(k => k.RemoteIPEndPoint.Equals(_session.RemoteIPEndPoint) && k.InstantKey != _request.InstantKey).Select(k => k);

            foreach (var key in duplicatedKeys)
            {
                GameServerInfoRedisOperator.DeleteKeyValue(key);
            }

            var fullKey = new GameServerInfoRedisKey()
            {
                ServerID = ServerFactory.Server.ServerID,
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                GameName = _request.GameName,
                InstantKey = _request.InstantKey
            };
            //we check if the database have multiple game server if it contains
            if (matchedKeys.Where(k => k.FullKey == fullKey.FullKey).Count() == 1)
            {
                _gameServerInfo = _redisClient.Values.Where(x =>
                                    x.ServerID == ServerFactory.Server.ServerID &
                                    x.RemoteIPEndPoint == _session.RemoteIPEndPoint &
                                    x.InstantKey == _request.InstantKey &
                                    x.GameName == _request.GameName)
                                    .FirstOrDefault();
            }
            else
            {
                _gameServerInfo = new GameServerInfo2()
                {
                    ServerID = ServerFactory.Server.ServerID,
                    RemoteIPEndPoint = _session.RemoteIPEndPoint,
                    GameName = _request.GameName,
                    InstantKey = _request.InstantKey,
                    ServerStatus = GameServerStatus.Normal,
                    LastPacketReceivedTime = DateTime.Now
                };
            }
        }
    }
}
