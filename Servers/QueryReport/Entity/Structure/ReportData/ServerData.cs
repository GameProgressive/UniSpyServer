using System.Collections.Generic;
using System.Net;
using QueryReport.Application;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerData
    {
        private static readonly List<string> StandardServerKeys =
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
            string[] kvArray = serverData.Split("\0");

            for (int i = 0; i < kvArray.Length; i += 2)
            {
                if (KeyValue.ContainsKey(kvArray[i]))
                {
                    if (KeyValue[kvArray[i]] == kvArray[i + 1])
                    {
                        continue;
                    }
                    else
                    {
                        ServerManager.LogWriter.Log.Fatal("Same key with different value has recieved!!!");
                    }
                }

                KeyValue.Add(kvArray[i], kvArray[i + 1]);
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
