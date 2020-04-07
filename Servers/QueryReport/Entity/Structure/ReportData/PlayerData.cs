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

        public List<Dictionary<string, string>> KeyValue { get; protected set; }

        public PlayerData()
        {
            KeyValue = new List<Dictionary<string, string>>();
        }

        public void Update(string playerData)
        {
            KeyValue.Clear();
            int playerCount = System.Convert.ToInt32(playerData[0]);
            playerData = playerData.Substring(1);
            //only store first 2 arrays
            string[] keyValue = playerData.Split("\0\0").Take(2).ToArray();

            List<string> keys = keyValue[0].Split("\0")
                 .Where(ks => ks.Contains("_", System.StringComparison.Ordinal))
                 .ToList();

            List<string> values = keyValue[1].Split("\0").ToList();

            for (int i = 0; i < playerCount; i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();

                for (int j = 0; j < keys.Count; j++)
                {
                    temp.Add(keys[j], values[i * keys.Count + j]);
                }

                KeyValue.Add(temp);
            }
        }
    }
}
