using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class TeamKeyValue
    {
        public List<Dictionary<string, string>> Data = new List<Dictionary<string, string>>();

        public void Update(string teamData)
        {
            Data.Clear();
            int teamCount = System.Convert.ToInt32(teamData[0]);
            teamData = teamData.Substring(1);
            string[] dataPartition = teamData.Split("\x00\x00");
            string[] keys = dataPartition[0].Split("\x00");
            string[] values = dataPartition[1].Split("\x00");

            for (int i = 0; i < teamCount; i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();
                for (int j = 0; j < keys.Length; j++)
                {
                    temp.Add(keys[j], values[i * keys.Length + j]);
                }
                Data.Add(temp);
            }
        }
    }
}
