using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;

namespace QueryReport.Entity.Structure.ReportData
{
    public class TeamData
    {
        public List<Dictionary<string, string>> KeyValueList { get; protected set; }

        private readonly List<string> StandardTeamKey =
            new List<string> { "team_t", "score_t", "nn_groupid" };

        public TeamData()
        {
            KeyValueList = new List<Dictionary<string, string>>();
        }

        public void Update(string teamData)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                    StringExtensions.ReplaceUnreadableCharToHex(teamData));

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
                for (int keyIndex = 0; keyIndex < keys.Count(); keyIndex++)
                {
                    if (keyValue.ContainsKey(keys[keyIndex] + teamIndex.ToString()))
                    {
                        LogWriter.ToLog($"Ignoring same player key value {keys[keyIndex]} : {values[teamIndex * keys.Count + keyIndex]}");
                        continue;
                    }

                    keyValue.Add(keys[keyIndex] + teamIndex.ToString(), values[teamIndex * keys.Count + keyIndex]);
                    LogWriter.ToLog(LogEventLevel.Verbose, $"{keys[keyIndex] + teamIndex.ToString()}:{values[teamIndex * keys.Count + keyIndex]}");
                }

                KeyValueList.Add(keyValue);

            }
        }
    }
}
