using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Logging;

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
            KeyValueList.Clear();
            int playerCount = System.Convert.ToInt32(playerData[0]);
            playerData = playerData.Substring(1);
            //only store first 2 arrays
            string[] keyValueArray = playerData.Split("\0\0").Take(2).ToArray();

            List<string> keys = keyValueArray[0].Split("\0")
                 .Where(ks => ks.Contains("_", System.StringComparison.Ordinal))
                 .ToList();

            List<string> values = keyValueArray[1].Split("\0").ToList();

            for (int i = 0; i < playerCount; i++)
            {
                Dictionary<string, string> keyValue = new Dictionary<string, string>();

                for (int j = 0; j < keys.Count; j++)
                {
                    //we do not know why same key appears in key list
                    //wrong implementing of GameSpySDK
                    if (keyValue.ContainsKey(keys[j]+i.ToString()))
                    {
                        LogWriter.ToLog($"Ignoring same player key value {keys[j]} : {values[i * keys.Count + j]}");
                        continue;
                    }
                    keyValue.Add(keys[j]+i.ToString(), values[i * keys.Count + j]);
                }

                KeyValueList.Add(keyValue);
            }
        }
    }
}
