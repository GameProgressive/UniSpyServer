using GameSpyLib.Common.Entity.Interface;
using QueryReport.Entity.Enumerator;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler : QRCommandHandlerBase
    {
        private GameServer _gameServer;
        private HeartBeatReportType _reportType;
        private string _dataPartition, _serverData, _playerData, _teamData;
        private int _playerPos, _teamPos;
        private int _playerLenth, _teamLength;

        public HeartBeatHandler(ISession session, byte[] recv) : base(session, recv)
        {
        }

        protected override void CheckRequest()
        {
            BasePacket basePacket = new BasePacket();

            if (!basePacket.Parse(_recv))
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }

            //Save server information.
            _dataPartition = Encoding.ASCII.GetString(_recv.Skip(5).ToArray());

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

                //if (PeerGroup.PeerGroupKeyList.Contains(_gameServer.ServerData.KeyValue["gamename"])
                //     && !_gameServer.ServerData.KeyValue.ContainsKey("hostport"))
                //{
                //    _gameServer.IsPeerServer = true;
                //}
                //GameServer.DeleteServer(
                //    endPoint,
                //    _gameServer.ServerData.KeyValue["gamename"]
                //    );
            }
            else
            {
                _errorCode = QRErrorCode.Parse;
                return;
            }


            base.CheckRequest();
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

            GameServer.UpdateServer(
               _session.RemoteEndPoint,
               _gameServer.ServerData.KeyValue["gamename"],
               _gameServer
           );
        }

        protected override void ConstructeResponse()
        {
            ChallengePacket packet = new ChallengePacket();
            packet.Parse(_session.RemoteEndPoint, _recv);
            _sendingBuffer = packet.BuildResponse();
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

            string gameServerRedisKey = GameServer.GenerateKey(_session.RemoteEndPoint, gameName);

            string address = ((IPEndPoint)_session.RemoteEndPoint).Address.ToString();
            //make sure one ip address create one server on each game
            List<string> redisSimilarKeys =
                GameServer.GetMatchedKeys($"{address}*{gameName}");

            //we check if the database have multiple game server if it contains
            if (redisSimilarKeys.Contains(gameServerRedisKey))
            {
                //save remote server data to local
                _gameServer = GameServer.GetServers(gameServerRedisKey).First();
                //delete all servers except this server
                foreach (var key in redisSimilarKeys)
                {
                    if (key == gameServerRedisKey)
                    {
                        continue;
                    }
                    GameServer.DeleteServer(key);
                }
            }
            else //redis do not have this server we create then update
            {
                _gameServer = new GameServer();
                _gameServer.Parse(_session.RemoteEndPoint);
            }
        }
    }
}
