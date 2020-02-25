using GameSpyLib.Common;
using System.Collections.Generic;
using System.Linq;

namespace QueryReport.Entity.Structure.ReportData
{
    public class PlayerInfo
    {
        public List<Dictionary<string, string>> Data = new List<Dictionary<string, string>>();
        public void UpdatePlayerInfo(string playerData)
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
                    temp.Add(keys[j], values[i * keys.Length + j]);
                }
                Data.Add(temp);
            }

        }
    }
}
