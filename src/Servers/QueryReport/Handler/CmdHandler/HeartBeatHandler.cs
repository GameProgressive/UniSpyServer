using System;
using System.Collections.Generic;
using System.Linq;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Application;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure.Redis;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Handler.CmdHandler
{
    internal sealed class HeartBeatHandler : QRCmdHandlerBase
    {
        private new HeartBeatRequest _request => (HeartBeatRequest)base._request;
        private GameServerInfo _gameServerInfo;
        private new HeartBeatResult _result
        {
            get => (HeartBeatResult)base._result;
            set => base._result = value;
        }
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
                    _gameServerInfo.ServerData.Update(_request.ServerData);
                    _gameServerInfo.PlayerData.Update(_request.PlayerData);
                    _gameServerInfo.TeamData.Update(_request.TeamData);
                    break;
                case HeartBeatReportType.ServerPlayerData:
                    _gameServerInfo.ServerData.Update(_request.ServerData);
                    _gameServerInfo.PlayerData.Update(_request.PlayerData);
                    _gameServerInfo.LastPacketReceivedTime = DateTime.Now;
                    break;
                case HeartBeatReportType.ServerData:
                    _gameServerInfo.ServerData.Update(_request.ServerData);
                    _gameServerInfo.LastPacketReceivedTime = DateTime.Now;
                    break;
            }

            UpdateGameServerByState();
            //parse the endpoint information into result class
            _result.RemoteEndPoint = _session.RemoteEndPoint;
        }

        private void UpdateGameServerByState()
        {
            var fullKey = new GameServerInfoRedisKey()
            {
                ServerID = QRServerFactory.Server.ServerID,
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                InstantKey = _request.InstantKey,
                GameName = _gameServerInfo.ServerData.KeyValue["gamename"]
            };
            _session.InstantKey = _request.InstantKey;
            if (_gameServerInfo.ServerData.ServerStatus == GameServerServerStatus.Shutdown)
            {
                GameServerInfoRedisOperator.DeleteKeyValue(fullKey);
            }
            else
            {
                GameServerInfoRedisOperator.SetKeyValue(fullKey, _gameServerInfo);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new HeartBeatResponse(_request, _result);
        }

        private void CheckSpamGameServer()
        {
            List<string> tempKeyVal = _request.DataPartition.Split('\0').ToList();
            int indexOfGameName = tempKeyVal.IndexOf("gamename");
            string gameName = tempKeyVal[indexOfGameName + 1];

            var fullKey = new GameServerInfoRedisKey()
            {
                ServerID = QRServerFactory.Server.ServerID,
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                GameName = gameName
            };

            //make sure one ip address create one server on each game
            var searchKey = new GameServerInfoRedisKey()
            {
                RemoteIPEndPoint = _session.RemoteIPEndPoint,
                GameName = gameName
            };

            var matchedKeys =
                GameServerInfoRedisOperator.GetMatchedKeys(searchKey);

            //we check if the database have multiple game server if it contains
            if (matchedKeys.Where(k => k.RedisFullKey == fullKey.RedisFullKey).Count() == 1)
            {
                _gameServerInfo = GameServerInfoRedisOperator.GetSpecificValue(fullKey);
            }
            else
            {
                _gameServerInfo = new GameServerInfo(_session.RemoteIPEndPoint);
            }
        }
    }
}
