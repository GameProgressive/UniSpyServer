using UniSpyServer.QueryReport.Abstraction.BaseClass;
using UniSpyServer.QueryReport.Entity.contract;
using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.QueryReport.Entity.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Encryption;

namespace UniSpyServer.QueryReport.Entity.Structure.Request
{
    [RequestContract(RequestType.HeartBeat)]
    public sealed class HeartBeatRequest : RequestBase
    {
        public string ServerData { get; private set; }
        public string PlayerData { get; private set; }
        public string TeamData { get; private set; }
        public string GameName
        {
            get
            {
                List<string> tempKeyVal = DataPartition.Split('\0').ToList();
                int indexOfGameName = tempKeyVal.IndexOf("gamename");
                return tempKeyVal[indexOfGameName + 1];
            }
        }
        public string DataPartition { get; private set; }
        public HeartBeatReportType ReportType { get; set; }

        public HeartBeatRequest(object rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            int playerPos, teamPos;
            int playerLenth, teamLength;
            DataPartition = UniSpyEncoding.GetString(RawRequest.Skip(5).ToArray());

            playerPos = DataPartition.IndexOf("player_\0", StringComparison.Ordinal);
            teamPos = DataPartition.IndexOf("team_t\0", StringComparison.Ordinal);

            if (playerPos != -1 && teamPos != -1)
            {
                ReportType = HeartBeatReportType.ServerTeamPlayerData;
                playerLenth = teamPos - playerPos;
                teamLength = DataPartition.Length - teamPos;

                ServerData = DataPartition.Substring(0, playerPos - 4);
                PlayerData = DataPartition.Substring(playerPos - 1, playerLenth - 2);
                TeamData = DataPartition.Substring(teamPos - 1, teamLength);
            }
            else if (playerPos != -1)
            {
                //normal heart beat
                ReportType = HeartBeatReportType.ServerPlayerData;
                playerLenth = DataPartition.Length - playerPos;
                ServerData = DataPartition.Substring(0, playerPos - 4);
                PlayerData = DataPartition.Substring(playerPos - 1, playerLenth);
            }
            else if (playerPos == -1 && teamPos == -1)
            {
                ReportType = HeartBeatReportType.ServerData;
                ServerData = DataPartition;
            }
            else
            {
                throw new QRException("HeartBeat request is invalid.");
            }
        }
    }
}
