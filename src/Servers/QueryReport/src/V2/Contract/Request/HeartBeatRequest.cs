using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.QueryReport.V2.Abstraction.BaseClass;
using UniSpy.Server.QueryReport.V2.Enumerate;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.QueryReport.V2.Contract.Request
{

    public sealed class HeartBeatRequest : RequestBase
    {
        public Dictionary<string, string> ServerData { get; private set; }
        public List<Dictionary<string, string>> PlayerData { get; private set; }
        public List<Dictionary<string, string>> TeamData { get; private set; }
        public GameServerStatus ServerStatus { get; private set; }
        public uint? GroupId { get; private set; }
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
        // public HeartBeatReportType ReportType { get; set; }

        public HeartBeatRequest(byte[] rawRequest) : base(rawRequest)
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
            // todo if there is no server data, we whether need to throw exception?
            if (playerPos != -1 && teamPos != -1)
            {
                // ReportType = HeartBeatReportType.ServerTeamPlayerData;
                playerLenth = teamPos - playerPos;
                teamLength = DataPartition.Length - teamPos;

                var serverDataStr = DataPartition.Substring(0, playerPos - 4);
                ParseServerData(serverDataStr);
                var playerDataStr = DataPartition.Substring(playerPos - 1, playerLenth - 2);
                ParsePlayerData(playerDataStr);
                var teamDataStr = DataPartition.Substring(teamPos - 1, teamLength);
                ParseTeamData(teamDataStr);
            }
            else if (playerPos != -1)
            {
                //normal heart beat
                // ReportType = HeartBeatReportType.ServerPlayerData;
                playerLenth = DataPartition.Length - playerPos;
                var serverDataStr = DataPartition.Substring(0, playerPos - 4);
                ParseServerData(serverDataStr);
                var playerDataStr = DataPartition.Substring(playerPos - 1, playerLenth);
                ParsePlayerData(playerDataStr);
            }
            else if (playerPos == -1 && teamPos == -1)
            {
                // ReportType = HeartBeatReportType.ServerData;
                var serverDataStr = DataPartition;
                ParseServerData(serverDataStr);
            }
            else
            {
                throw new QueryReport.Exception("HeartBeat request is invalid.");
            }

            if (ServerData.ContainsKey("groupid"))
            {
                if (!uint.TryParse(ServerData["groupid"], out var groupId))
                {
                    throw new QueryReport.Exception("GroupId is invalid.");

                }
                GroupId = groupId;
            }
        }
        private void ParseServerData(string serverDataStr)
        {
            ServerData = new Dictionary<string, string>();
            string[] keyValueArray = serverDataStr.Split("\0");

            for (int i = 0; i < keyValueArray.Length; i += 2)
            {
                if (i + 2 > keyValueArray.Length)
                {
                    break;
                }

                string tempKey = keyValueArray[i];
                string tempValue = keyValueArray[i + 1];

                if (tempKey == "")
                {
                    LogWriter.LogVerbose("Skiping empty key value");
                    continue;
                }
                // no matter happens we just update the key value
                if (ServerData.ContainsKey(tempKey))
                {
                    ServerData[tempKey] = tempValue;
                }
                else
                {
                    ServerData.Add(tempKey, tempValue);
                }
            }
            if (!ServerData.ContainsKey("statechanged"))
            {
                ServerStatus = GameServerStatus.Normal;
            }
            else
            {
                ServerStatus = (GameServerStatus)Enum.Parse(typeof(GameServerStatus), ServerData?["statechanged"]);
            }
        }
        private void ParsePlayerData(string playerDataStr)
        {
            PlayerData = new List<Dictionary<string, string>>();
            // _client.LogInfo(StringExtensions.ReplaceUnreadableCharToHex(playerDataStr));
            //TODO check if each update contains all player information
            int playerCount = Convert.ToInt32(playerDataStr[0]);
            playerDataStr = playerDataStr.Substring(1);

            //we first read the key index
            int IndexOfKey = playerDataStr.IndexOf("\0\0", StringComparison.Ordinal);
            //then get all the keys
            string keyStr = playerDataStr.Substring(0, IndexOfKey);
            List<string> keys = keyStr.Split('\0', StringSplitOptions.RemoveEmptyEntries).ToList();

            string valuesStr = playerDataStr.Substring(IndexOfKey + 2);
            List<string> values = valuesStr.Split('\0').ToList();

            //according to player total number and key total number to add the data into list
            for (int playerIndex = 0; playerIndex < playerCount; playerIndex++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int keyIndex = 0; keyIndex < keys.Count; keyIndex++)
                {
                    string tempKey = keys[keyIndex] + playerIndex.ToString();
                    string tempValue = values[playerIndex * keys.Count + keyIndex];
                    if (keyValue.ContainsKey(tempKey))
                    {
                        keyValue[tempKey] = tempValue;
                    }
                    else
                    {
                        keyValue.Add(tempKey, tempValue);
                    }
                }
                PlayerData.Add(keyValue);
            }
        }
        private void ParseTeamData(string teamDataStr)
        {
            TeamData = new List<Dictionary<string, string>>();
            // _client.LogInfo(// StringExtensions.ReplaceUnreadableCharToHex(teamDataStr));
            //TODO check if each update contains all team information

            int teamCount = System.Convert.ToInt32(teamDataStr[0]);
            teamDataStr = teamDataStr.Substring(1);

            int endKeyIndex = teamDataStr.IndexOf("\0\0", System.StringComparison.Ordinal);
            string keyStr = teamDataStr.Substring(0, endKeyIndex);
            List<string> keys = keyStr.Split('\0', System.StringSplitOptions.RemoveEmptyEntries).ToList();

            string valueStr = teamDataStr.Substring(endKeyIndex + 2);
            List<string> values = valueStr.Split('\0').ToList();

            for (int teamIndex = 0; teamIndex < teamCount; teamIndex++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();
                // iterate the index get the keys and values
                for (int keyIndex = 0; keyIndex < keys.Count(); keyIndex++)
                {
                    string tempKey = keys[keyIndex] + teamIndex.ToString();
                    string tempValue = values[teamIndex * keys.Count + keyIndex];
                    if (keyValue.ContainsKey(tempKey))
                    {
                        keyValue[tempKey] = tempValue;
                    }
                    else
                    {
                        keyValue.Add(tempKey, tempValue);
                    }
                    //LogWriter.ToLog(LogEventLevel.Verbose, $"Updated new team key value {tempKey}:{tempValue}");
                }
                TeamData.Add(keyValue);
            }
        }
    }
}
