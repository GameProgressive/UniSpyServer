using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class PlayerInfo
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public void Update(string playerData)
        {
            Data.Clear();
            int playerCount = System.Convert.ToInt32(playerData[0]);
            playerData = playerData.Substring(1);
            string[] dataPartition = playerData.Split("\x00\x00");
            string[] keys = dataPartition[0].Split("\x00");
            string[] values = dataPartition[1].Split("\x00");

            for (int i = 0; i < playerCount; i++)
            {
                Dictionary<string, string> temp = new Dictionary<string, string>();
                for (int j = 0; j < keys.Length; j++)
                {
                    Data.Add(keys[j], values[i * keys.Length + j]);
                }
            }

        }
    }
}
