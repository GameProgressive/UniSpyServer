using GameSpyLib.Common;
using System.Collections.Generic;
using System.Net;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerKeyValue
    {
        public Dictionary<string, string> Data = new Dictionary<string, string>();

        public void Update(string serverData,EndPoint endPoint)
        {
            Data.Clear();
            string[] parts = serverData.Split("\x00");
            Data = GameSpyUtils.ConvertRequestToKeyValue(parts);
            //todo add the location
            //Data.Add("regionname","");
            //Data.Add("country","");
        }
    }
}
