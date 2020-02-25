using GameSpyLib.Common;
using QueryReport.Entity.Structure.Packet;
using QueryReport.Entity.Structure.ReportData;
using QueryReport.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace QueryReport.Handler.CommandHandler.HeartBeat
{
    public class HeartBeatHandler : QRHandlerBase
    {
        string _heartBeatHeader;
        string _dataFrag;
        uint _playerTeamCount;
        public HeartBeatHandler(QRServer server, EndPoint endPoint, byte[] recv) : base(server, endPoint, recv)
        {
        }


        protected override void CheckRequest(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO

            base.CheckRequest(server, endPoint, recv);
        }

        protected override void DataOperation(QRServer server, EndPoint endPoint, byte[] recv)
        {
            //TODO
            //Save server information.
            string dataPartition = Encoding.ASCII.GetString(recv.Skip(5).ToArray());
            string serverData, playerData, teamData;
            int playerPos = dataPartition.IndexOf("player_");
            if (playerPos != -1)
            {
                serverData = dataPartition.Substring(0, playerPos - 4); 
            }
            
            int teamPos = dataPartition.IndexOf("team_t");
            int playerLenth = teamPos - playerPos;
            if (teamPos != -1)
            {
                playerData = dataPartition.Substring(playerPos - 1, playerLenth - 2);

                int teamLength = dataPartition.Length - teamPos;
                teamData = dataPartition.Substring(teamPos - 1, teamLength);

            }
        }

        protected override void ConstructeResponse(QRServer server, EndPoint endPoint, byte[] recv)
        {
            ChallengePacket challenge = new ChallengePacket(endPoint, recv);
            _sendingBuffer = challenge.GenerateResponse();
        }
    }
}
