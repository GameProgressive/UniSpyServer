using GameSpyLib.Common.Entity.Interface;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler : QRCommandHandlerBase
    {
        private GameServer _gameServer;

        public HeartBeatHandler(IClient client, byte[] recv) : base(client, recv)
        {
        }

        protected override void CheckRequest()
        {
            BasePacket basePacket = new BasePacket();
            basePacket.Parse(_recv);
            //DedicatedGameServer gameServer = new DedicatedGameServer();
            //gameServer.Parse(endPoint, basePacket.InstantKey);

            _gameServer = new GameServer();
            QRClient client = (QRClient)_client.GetInstance();
            _gameServer.Parse(client.RemoteEndPoint, basePacket.InstantKey);
            //_gameServer = QRServer.GameServerList.GetOrAdd(endPoint, gameServer);

            base.CheckRequest();
        }

        protected override void DataOperation()
        {


            //Save server information.
            string dataPartition = Encoding.ASCII.GetString(_recv.Skip(5).ToArray());
            string serverData, playerData, teamData;

            int playerPos = dataPartition.IndexOf("player_\0", StringComparison.Ordinal);

            int teamPos = dataPartition.IndexOf("team_t\0", StringComparison.Ordinal);

            if (playerPos != -1 && teamPos != -1)
            {
                //normal heart beat
                int playerLenth = teamPos - playerPos;
                int teamLength = dataPartition.Length - teamPos;

                serverData = dataPartition.Substring(0, playerPos - 4);
                playerData = dataPartition.Substring(playerPos - 1, playerLenth - 2);
                teamData = dataPartition.Substring(teamPos - 1, teamLength);

                _gameServer.ServerData.Update(serverData);
                _gameServer.PlayerData.Update(playerData);
                _gameServer.TeamData.Update(teamData);
                _gameServer.LastPacket = DateTime.Now;
            }
            else if (playerPos != -1)
            {
                //normal heart beat
                int playerLenth = dataPartition.Length - playerPos;
                serverData = dataPartition.Substring(0, playerPos - 4);
                playerData = dataPartition.Substring(playerPos - 1, playerLenth);
                _gameServer.ServerData.Update(serverData);
                _gameServer.PlayerData.Update(playerData);
                _gameServer.LastPacket = DateTime.Now;
            }
            else
            {
                //shutdown heart beat
                serverData = dataPartition;
                _gameServer.ServerData.Update(serverData);
                //if (PeerGroup.PeerGroupKeyList.Contains(_gameServer.ServerData.KeyValue["gamename"])
                //     && !_gameServer.ServerData.KeyValue.ContainsKey("hostport"))
                //{
                //    _gameServer.IsPeerServer = true;
                //}
                //GameServer.DeleteServer(
                //    endPoint,
                //    _gameServer.ServerData.KeyValue["gamename"]
                //    );
                return;
            }

            ////make sure one ip address create one server
            //List<string> redisKeys = GameServer.GetMatchedKeys(((IPEndPoint)endPoint).Address + "*" + _gameServer.ServerData.KeyValue["gamename"]);

            //foreach (var key in redisKeys)
            //{
            //    if (key == GameServer.GenerateKey(endPoint, _gameServer.ServerData.KeyValue["gamename"]))
            //    {
            //        continue;
            //    }
            //    GameServer.DeleteServer(key);
            //}
            QRClient client = (QRClient)_client.GetInstance();
            GameServer.UpdateServer(
               client.RemoteEndPoint,
               _gameServer.ServerData.KeyValue["gamename"],
               _gameServer
           );
        }

        protected override void ConstructeResponse()
        {
            ChallengePacket packet = new ChallengePacket();
            QRClient client = (QRClient)_client.GetInstance();
            packet.Parse(client.RemoteEndPoint, _recv);
            _sendingBuffer = packet.GenerateResponse();
        }
    }
}
