using QueryReport.Abstraction.BaseClass;
using QueryReport.Entity.Enumerate;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Request;
using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            switch ((HeartBeatReportType)_result.PacketType)
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
                    _gameServerInfo.LastPacket = DateTime.Now;
                    break;
                case HeartBeatReportType.ServerData:
                    _gameServerInfo.ServerData.Update(_request.ServerData);
                    _gameServerInfo.LastPacket = DateTime.Now;
                    break;
            }

            UpdateGameServerByState();
            //parse the endpoint information into result class
            _result.RemoteEndPoint = _session.RemoteEndPoint;
        }

        private void UpdateGameServerByState()
        {
            var fullKey = GameServerInfo.RedisOperator.BuildFullKey(
                _session.RemoteIPEndPoint, 
                _gameServerInfo.ServerData.KeyValue["gamename"]);
            if (_gameServerInfo.ServerData.ServerStatus == GameServerServerStatus.Shutdown)
            {
                GameServerInfo.RedisOperator.DeleteKeyValue(fullKey);
            }
            else
            {
                GameServerInfo.RedisOperator.SetKeyValue(fullKey, _gameServerInfo);
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

            string fullKey = GameServerInfo.RedisOperator.BuildFullKey(_session.RemoteIPEndPoint, gameName);

            //make sure one ip address create one server on each game
            string searchKey = GameServerInfo.RedisOperator.BuildSearchKey(_session.RemoteIPEndPoint, gameName);
            List<string> matchedKeys =
                GameServerInfo.RedisOperator.GetMatchedKeys(searchKey);

            //we check if the database have multiple game server if it contains
            if (matchedKeys.Contains(fullKey))
            {
                //save remote server data to local
                _gameServerInfo = GameServerInfo.RedisOperator.GetSpecificValue(fullKey);
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
                _gameServerInfo = new GameServerInfo(_session.RemoteEndPoint);
            }
        }
    }
}
