using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryReport.Entity.Structure.ReportData
{
    public class PlayerData
    {
        private readonly List<string> StandardPlayerKeys =
            new List<string>
            {
                "groupid","player_","score_","skill_","ping_","team_","deaths_","pid_"
            };

        public List<Dictionary<string, string>> KeyValueList { get; protected set; }

        public PlayerData()
        {
            KeyValueList = new List<Dictionary<string, string>>();
        }

        public void Update(string playerData)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                StringExtensions.ReplaceUnreadableCharToHex(playerData));

            KeyValueList.Clear();
            int playerCount = Convert.ToInt32(playerData[0]);
            playerData = playerData.Substring(1);

            //we first read the key index
            int IndexOfKey = playerData.IndexOf("\0\0", StringComparison.Ordinal);
            //then get all the keys
            string keyStr = playerData.Substring(0, IndexOfKey);
            List<string> keys = keyStr.Split('\0', StringSplitOptions.RemoveEmptyEntries).ToList();

            string valuesStr = playerData.Substring(IndexOfKey+2);
            List<string> values = valuesStr.Split('\0').ToList();

            //according to player total number and key total number to add the data into list
            for (int playerIndex = 0; playerIndex < playerCount; playerIndex++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int keyIndex = 0; keyIndex < keys.Count; keyIndex++)
                {
                    if (keyValue.ContainsKey(keys[keyIndex] + playerIndex.ToString()))
                    {
                        LogWriter.ToLog($"Ignoring same player key value {keys[keyIndex]} : {values[playerIndex * keys.Count + keyIndex]}");
                        continue;
                    }
                    keyValue.Add(keys[keyIndex]+playerIndex.ToString(), values[playerIndex * keys.Count + keyIndex]);
                    LogWriter.ToLog(LogEventLevel.Verbose, $"{keys[keyIndex] + playerIndex.ToString()}:{values[playerIndex * keys.Count + keyIndex]}");
                }
                KeyValueList.Add(keyValue);
            } 
        }
    }
}
