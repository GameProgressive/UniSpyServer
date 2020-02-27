using GameSpyLib.Common;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerInfo
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public void Update(string serverData)
        {
            Data.Clear();
            string[] parts = serverData.Split("\x00");
            Data = GameSpyUtils.ConvertRequestToKeyValue(parts);
        }
    }
}
