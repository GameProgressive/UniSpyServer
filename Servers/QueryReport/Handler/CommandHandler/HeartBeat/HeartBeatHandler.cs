using Newtonsoft.Json;
using QueryReport.Application;
using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler : CommandHandlerBase
    {
        private GameServer _gameServer;

        public HeartBeatHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }

        protected override void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv)
        {
            BasePacket basePacket = new BasePacket();
            basePacket.Parse(recv);
            GameServer gameServer = new GameServer();
            gameServer.Parse(endPoint, basePacket.InstantKey);

            _gameServer = QRServer.GameServerList.GetOrAdd(endPoint, gameServer);

            base.CheckRequest(server, endPoint, recv);
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //Save server information.
            string dataPartition = Encoding.ASCII.GetString(recv.Skip(5).ToArray());
            string serverData, playerData, teamData;

            int playerPos = dataPartition.IndexOf("player_", StringComparison.Ordinal);

            int teamPos = dataPartition.IndexOf("team_t\0", StringComparison.Ordinal);

            if (playerPos != -1 && teamPos != -1)
            {
                //normal heart beat
                int playerLenth = teamPos - playerPos;
                int teamLength = dataPartition.Length - teamPos;

                serverData = dataPartition.Substring(0, playerPos - 4);
                playerData = dataPartition.Substring(playerPos - 1, playerLenth - 2);
                teamData = dataPartition.Substring(teamPos - 1, teamLength);

                _gameServer.ServerData.Update(serverData, endPoint);
                _gameServer.PlayerData.Update(playerData);
                _gameServer.TeamData.Update(teamData);
                _gameServer.LastHeartBeatPacket = DateTime.Now;
            }
            else
            {
                //shutdown heart beat
                _gameServer.IsValidated = false;
            }
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            ChallengePacket packet = new ChallengePacket();
            packet.Parse(endPoint, recv);
            _sendingBuffer = packet.GenerateResponse();
        }
    }
}
