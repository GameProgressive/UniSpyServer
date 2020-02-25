using GameSpyLib.Common;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerInfo
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();
        public void UpdateServerInfo(string serverData)
        {
            Data.Clear();
            string[] parts = serverData.Split("\x00");
            Data = GameSpyUtils.ConvertRequestToKeyValue(parts);
        }
    }
}
