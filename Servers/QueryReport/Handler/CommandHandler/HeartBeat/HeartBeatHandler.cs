using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
using System;
using System.Linq;
using System.Net;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler : QRHandlerBase
    {
        private GameServer _gameServer;
        public HeartBeatHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        { }


        protected override void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv)
        {
            BasePacket basePacket = new BasePacket(recv);
            _gameServer = QRServer.GameServerList.GetOrAdd(endPoint, new GameServer(endPoint, basePacket.InstantKey));
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

                _gameServer.Server.Update(serverData,endPoint);
                _gameServer.Player.Update(playerData);
                _gameServer.Team.Update(teamData);
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
            ChallengePacket challenge = new ChallengePacket(endPoint, recv);
            _sendingBuffer = challenge.GenerateResponse();
        }
    }
}
