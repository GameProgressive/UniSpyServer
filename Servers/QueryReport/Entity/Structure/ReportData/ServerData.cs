using System.Collections.Generic;
using System.Net;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerData
    {
        private static readonly List<string> StandardKeys =
            new List<string>
            {
                "hostname ", "gamever", "hostport", "mapname",
                "gametype", "gamevariant ","numplayers","numteams","maxplayers",
                "gamemode","teamplay","fraglimit","teamfraglimit","timelimit","timeelapsed",
                "roundtime","roundelapsed","password","groupid",
                "localip0","localport","natneg","statechanged","gamename","hostname"
            };

        public Dictionary<string, string> StandardKeyValue { get; protected set; }

        public Dictionary<string, string> CustomKeyValue { get; protected set; }

        public ServerData()
        {
            StandardKeyValue = new Dictionary<string, string>();
            CustomKeyValue = new Dictionary<string, string>();
        }

        public void Update(string serverData, EndPoint endPoint)
        {
            StandardKeyValue.Clear();
            CustomKeyValue.Clear();
            string[] keyValueArray = serverData.Split("\0", System.StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < keyValueArray.Length; j += 2)
            {
                if (StandardKeys.Contains(keyValueArray[j]))
                {
                    StandardKeyValue.Add(keyValueArray[j], keyValueArray[j + 1]);
                }
                else
                {
                    CustomKeyValue.Add(keyValueArray[j], keyValueArray[j + 1]);
                }
            }

            //todo add the location
            //Data.Add("regionname","");
            //Data.Add("country","");
        }
    }
}
