using System.Collections.Generic;
using System.Net;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerData
    {
        private static readonly List<string> GameSpyStandardKey =
            new List<string>
            {
                "hostname ", "gamever", "hostport", "mapname",
                "gametype", "gamevariant ","numplayers","numteams","maxplayers",
                "gamemode","teamplay","fraglimit","teamfraglimit","timelimit","timeelapsed",
                "roundtime","roundelapsed","password","groupid",
                "localip0","localport","natneg","statechanged","gamename","hostname"
            };

        public Dictionary<string, string> KeyValue { get; protected set; }

        public ServerData()
        {
            KeyValue = new Dictionary<string, string>();
        }

        public void Update(string serverData, EndPoint endPoint)
        {
            KeyValue.Clear();
            string[] keyValueArray = serverData.Split("\0", System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < keyValueArray.Length; i += 2)
            {
                KeyValue.Add(keyValueArray[i], keyValueArray[i + 1]);
            }

            //todo add the location
            //Data.Add("regionname","");
            //Data.Add("country","");
        }

        public void UpdateDictionary(string key, string value)
        {
            KeyValue.Add(key, value);
        }
    }
}
