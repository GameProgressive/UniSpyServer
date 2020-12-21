using UniSpyLib.Abstraction.Interface;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Structure.Response;

namespace QueryReport.Handler.CmdHandler
{
    public class HeartBeatHandler : QRCmdHandlerBase
    {
        protected GameServerInfo _gameServer;
        protected HeartBeatReportType _reportType;
        protected string _dataPartition, _serverData, _playerData, _teamData;
        protected int _playerPos, _teamPos;
        protected int _playerLenth, _teamLength;
        protected new HeartBeatRequest _request { get { return (HeartBeatRequest)base._request; } }

        public HeartBeatHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();

            //Save server information.
            _dataPartition = Encoding.ASCII.GetString(_request.RawRequest.Skip(5).ToArray());

            _playerPos = _dataPartition.IndexOf("player_\0", StringComparison.Ordinal);
            _teamPos = _dataPartition.IndexOf("team_t\0", StringComparison.Ordinal);

            if (_playerPos != -1 && _teamPos != -1)
            {
                _reportType = HeartBeatReportType.ServerTeamPlayerData;
            }
            else if (_playerPos != -1)
            {
                //normal heart beat
                _reportType = HeartBeatReportType.ServerPlayerData;
            }
            else if (_playerPos == -1 && _teamPos == -1)
            {
                _reportType = HeartBeatReportType.ServerData;
            }
            else
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }

            _session.InstantKey = _request.InstantKey;
        }

        protected override void DataOperation()
        {
            CheckSpamGameServer();

            switch (_reportType)
            {
                case HeartBeatReportType.ServerTeamPlayerData:
                    ParseServerTeamPlayerData();
                    break;
                case HeartBeatReportType.ServerPlayerData:
                    ParseServerPlayerData();
                    break;
                case HeartBeatReportType.ServerData:
                    ParseServerData();
                    break;
            }

            UpdateGameServerByState();
        }

        protected void UpdateGameServerByState()
        {
            var fullKey = GameServerInfo.RedisOperator.BuildFullKey(_session.RemoteIPEndPoint, _gameServer.ServerData.KeyValue["gamename"]);
            if (_gameServer.ServerData.ServerStatus == GameServerServerStatus.Shutdown)
            {
                GameServerInfo.RedisOperator.DeleteKeyValue(fullKey);
            }
            else
            {
                GameServerInfo.RedisOperator.SetKeyValue(fullKey, _gameServer);
            }
        }

        protected override void ConstructResponse()
        {
            HeartBeatResponse response = new HeartBeatResponse(_session, _request);
            _sendingBuffer = response.BuildResponse();
        }

        private void ParseServerTeamPlayerData()
        {
            //normal heart beat
            _playerLenth = _teamPos - _playerPos;
            _teamLength = _dataPartition.Length - _teamPos;

            _serverData = _dataPartition.Substring(0, _playerPos - 4);
            _playerData = _dataPartition.Substring(_playerPos - 1, _playerLenth - 2);
            _teamData = _dataPartition.Substring(_teamPos - 1, _teamLength);

            _gameServer.ServerData.Update(_serverData);
            _gameServer.PlayerData.Update(_playerData);
            _gameServer.TeamData.Update(_teamData);
            _gameServer.LastPacket = DateTime.Now;
        }

        private void ParseServerPlayerData()
        {
            _playerLenth = _dataPartition.Length - _playerPos;

            _serverData = _dataPartition.Substring(0, _playerPos - 4);
            _playerData = _dataPartition.Substring(_playerPos - 1, _playerLenth);

            _gameServer.ServerData.Update(_serverData);
            _gameServer.PlayerData.Update(_playerData);
            _gameServer.LastPacket = DateTime.Now;
        }

        private void ParseServerData()
        {
            _serverData = _dataPartition;
            _gameServer.ServerData.Update(_serverData);
        }

        private void CheckSpamGameServer()
        {
            List<string> tempKeyVal = _dataPartition.Split('\0').ToList();
            int indexOfGameName = tempKeyVal.IndexOf("gamename");
            string gameName = tempKeyVal[indexOfGameName + 1];

            string fullKey = GameServerInfo.RedisOperator.BuildFullKey(_session.RemoteIPEndPoint, gameName);

            //make sure one ip address create one server on each game
            string searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_session.RemoteIPEndPoint, gameName);
            List<string> matchedKeys =
                GameServerInfo.RedisOperator.GetMatchedKeys(searchKey);

            //we check if the database have multiple game server if it contains
            if (matchedKeys.Contains(fullKey))
            {
                //save remote server data to local
                _gameServer = GameServerInfo.RedisOperator.GetSpecificValue(fullKey);
                //delete all servers except this server
                foreach (var key in matchedKeys)
                {
                    if (key == fullKey)
                    {
                        continue;
                    }
                    GameServerInfo.RedisOperator.DeleteKeyValue(key);
                }
            }
            else //redis do not have this server we create then update
            {
                _gameServer = new GameServerInfo(_session.RemoteEndPoint);
            }
        }
    }
}
