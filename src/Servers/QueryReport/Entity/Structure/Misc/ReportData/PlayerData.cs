using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace QueryReport.Entity.Structure.ReportData
{
    public class PlayerData
    {
        public List<Dictionary<string, string>> KeyValueList { get; protected set; }

        public PlayerData()
        {
            KeyValueList = new List<Dictionary<string, string>>();
        }

        public void Update(string playerData)
        {
            LogWriter.Debug(StringExtensions.ReplaceUnreadableCharToHex(playerData));

            //TODO check if each update contains all player information
            KeyValueList.Clear();
            int playerCount = Convert.ToInt32(playerData[0]);
            playerData = playerData.Substring(1);

            //we first read the key index
            int IndexOfKey = playerData.IndexOf("\0\0", StringComparison.Ordinal);
            //then get all the keys
            string keyStr = playerData.Substring(0, IndexOfKey);
            List<string> keys = keyStr.Split('\0', StringSplitOptions.RemoveEmptyEntries).ToList();

            string valuesStr = playerData.Substring(IndexOfKey + 2);
            List<string> values = valuesStr.Split('\0').ToList();

            //according to player total number and key total number to add the data into list
            for (int playerIndex = 0; playerIndex < playerCount; playerIndex++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int keyIndex = 0; keyIndex < keys.Count; keyIndex++)
                {
                    string tempKey = keys[keyIndex] + playerIndex.ToString();
                    string tempValue = values[playerIndex * keys.Count + keyIndex];
                    // update existed key value
                    if (keyValue.ContainsKey(tempKey))
                    {
                        if (keyValue[tempKey] == tempValue)
                        {
                            //LogWriter.ToLog($"Ignoring same player key value {tempKey} : {tempValue}");
                        }
                        else
                        {
                            keyValue[tempKey] = tempValue;
                            //LogWriter.ToLog($"Updated player key value {tempKey} : {tempValue}");
                        }
                    }
                    else
                    {
                        keyValue.Add(tempKey, tempValue);
                        //LogWriter.ToLog(LogEventLevel.Verbose, $"Updated new player key value {tempKey}:{tempValue}");
                    }
                }
                KeyValueList.Add(keyValue);
            }
        }
    }
}
