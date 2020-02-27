using QueryReport.Entity.Structure;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Server;
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
            //TODO
            BaseResponsePacket b = new BaseResponsePacket(recv);
            _gameServer = QRServer.GameServerList.GetOrAdd(endPoint, new GameServer(endPoint, b.InstantKey));
            base.CheckRequest(server, endPoint, recv);
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //Save server information.
            string dataPartition = Encoding.ASCII.GetString(recv.Skip(5).ToArray());
            string serverData, playerData, teamData;

            int playerPos = dataPartition.IndexOf("player_");
            int teamPos = dataPartition.IndexOf("team_t");

            if (playerPos != -1 && teamPos != -1)
            {
                //normal heart beat
                serverData = dataPartition.Substring(0, playerPos - 4);
                _gameServer.ServerInfo.UpdateServerInfo(serverData);

                int playerLenth = teamPos - playerPos;
                playerData = dataPartition.Substring(playerPos - 1, playerLenth - 2);
                _gameServer.PlayerInfo.UpdatePlayerInfo(playerData);

                int teamLength = dataPartition.Length - teamPos;
                teamData = dataPartition.Substring(teamPos - 1, teamLength);
                _gameServer.TeamInfo.UpdatePlayerInfo(teamData);
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
