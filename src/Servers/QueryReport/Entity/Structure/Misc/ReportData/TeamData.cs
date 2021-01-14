using Serilog.Events;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Extensions;
using UniSpyLib.Logging;

namespace QueryReport.Entity.Structure.ReportData
{
    public class TeamData
    {
        public List<Dictionary<string, string>> KeyValueList { get; protected set; }

        public TeamData()
        {
            KeyValueList = new List<Dictionary<string, string>>();
        }

        public void Update(string teamData)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                    StringExtensions.ReplaceUnreadableCharToHex(teamData));
            //TODO check if each update contains all team information
            KeyValueList.Clear();

            int teamCount = System.Convert.ToInt32(teamData[0]);
            teamData = teamData.Substring(1);

            int endKeyIndex = teamData.IndexOf("\0\0", System.StringComparison.Ordinal);
            string keyStr = teamData.Substring(0, endKeyIndex);
            List<string> keys = keyStr.Split('\0', System.StringSplitOptions.RemoveEmptyEntries).ToList();

            string valueStr = teamData.Substring(endKeyIndex + 2);
            List<string> values = valueStr.Split('\0').ToList();

            for (int teamIndex = 0; teamIndex < teamCount; teamIndex++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();
                // iterate the index get the keys and values
                for (int keyIndex = 0; keyIndex < keys.Count(); keyIndex++)
                {
                    string tempKey = keys[keyIndex] + teamIndex.ToString();
                    string tempValue = values[teamIndex * keys.Count + keyIndex];
                    // if the key is existed in dictionary
                    //if (keyValue.ContainsKey(tempKey))
                    //{
                    //    // if the existed key value equal to key value in request, we do not update
                    //    if (keyValue[tempKey] == tempValue)
                    //    {
                    //        LogWriter.ToLog($"Ignoring same team key value {tempKey} : {tempValue}");
                    //    }
                    //    else
                    //    {
                    //        keyValue[tempKey] = tempValue;
                    //        LogWriter.ToLog($"Updated team key value {tempKey} : {tempValue}");
                    //    }

                    //    continue;
                    //}
                    //else
                    //{
                    keyValue.Add(tempKey, tempValue);
                    LogWriter.ToLog(LogEventLevel.Verbose, $"Updated new team key value {tempKey}:{tempValue}");
                    //}


                }

                KeyValueList.Add(keyValue);

            }
        }
    }
}
