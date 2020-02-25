using GameSpyLib.Common;
using System.Collections.Generic;
using System.Linq;

namespace QueryReport.Entity.Structure.ReportData
{
    public class PlayerInfo
    {
        public List<Dictionary<string, string>> Data = new List<Dictionary<string, string>>();
        public void SetPlayerInfo(string input)
        {
            int playerCount = System.Convert.ToInt32(input[0]);
            input = input.Substring(1);
            string[] dataPartition = input.Split("\x00\x00");
            string[] keys = dataPartition[0].Split("\x00");
            int keyNumber = keys.ToList().Count();
            string[] value = dataPartition[1].Split("\x00");


        }
    }
}
