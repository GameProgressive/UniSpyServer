using GameSpyLib.Common;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class TeamInfo
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public void SetPlayerInfo(string input)
        {
            string[] parts = input.Split("\x00");
            uint teamCount = System.Convert.ToUInt32(parts[0][0]);
            Data.Add("teamcount", teamCount.ToString());
            Data = GameSpyUtils.ConvertRequestToKeyValue(parts);
        }
    }
}
