using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class TeamData
    {
        public List<Dictionary<string, string>> StandardKeyValue { get; protected set; }

        public List<Dictionary<string, string>> CustomKeyValue { get; protected set; }

        private readonly List<string> StandardKey = new List<string> { "team_t", "score_t", "nn_groupid" };

        public TeamData()
        {
            StandardKeyValue = new List<Dictionary<string, string>>();
            CustomKeyValue = new List<Dictionary<string, string>>();
        }

        public void Update(string teamData)
        {
            StandardKeyValue.Clear();
            int teamCount = System.Convert.ToInt32(teamData[0]);
            teamData = teamData.Substring(1);
            string[] dataPartition = teamData.Split("\0\0", System.StringSplitOptions.RemoveEmptyEntries);
            string[] keys = dataPartition[0].Split("\0");
            string[] values = dataPartition[1].Split("\0");

            for (int i = 0; i < teamCount; i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();

                for (int j = 0; j < keys.Length; j++)
                {
                    temp.Add(keys[j], values[i * keys.Length + j]);
                }

                StandardKeyValue.Add(temp);
            }
        }
    }
}
