using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class PlayerData
    {
        private readonly List<string> StandardKey =
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
            string[] keyValue = playerData.Split("\0\0", System.StringSplitOptions.RemoveEmptyEntries);
            string[] keys = keyValue[0].Split("\0");
            string[] values = keyValue[1].Split("\0");

            for (int i = 0; i < playerCount; i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();

                for (int j = 0; j < keys.Length; j += 2)
                {
                    temp.Add(keys[j], values[i * keys.Length + j]);
                }

                KeyValue.Add(temp);
            }
        }
    }
}
