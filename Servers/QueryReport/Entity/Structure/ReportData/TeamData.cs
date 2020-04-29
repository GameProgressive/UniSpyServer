using GameSpyLib.Logging;
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
            KeyValueList.Clear();
            int teamCount = System.Convert.ToInt32(teamData[0]);
            teamData = teamData.Substring(1);
            string[] keyValueArray = teamData.Split("\0\0", System.StringSplitOptions.RemoveEmptyEntries);
            List<string> keys = keyValueArray[0].Split("\0").Where(k => k.Contains("_t")).ToList();
            List<string> values = keyValueArray[1].Split("\0").ToList();

            for (int i = 0; i < teamCount; i++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int j = 0; j < keys.Count; j++)
                {
                    //we do not know why same key appears in key list
                    //wrong implementing of GameSpySDK
                    if (keyValue.ContainsKey(keys[j] + i.ToString()))
                    {
                        LogWriter.ToLog($"Ignoring same team key value {keys[j]} : {values[i * keys.Count + j]}");
                        continue;
                    }

                    keyValue.Add(keys[j] + i.ToString(), values[i * keys.Count + j]);
                    LogWriter.ToLog(Serilog.Events.LogEventLevel.Verbose, $"{keys[j] + i.ToString()}:{values[i * keys.Count + j]}");
                }

                KeyValueList.Add(keyValue);
            }
        }
    }
}
