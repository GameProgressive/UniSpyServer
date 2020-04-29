using GameSpyLib.Logging;
using System.Collections.Generic;

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

        public void Update(string serverData)
        {
            KeyValue.Clear();
            string[] keyValueArray = serverData.Split("\0",System.StringSplitOptions.RemoveEmptyEntries);
            string tempDebug = "";
            for (int i = 0; i < keyValueArray.Length; i++)
            {
                tempDebug += keyValueArray[i] + @"\";
            }
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Verbose, tempDebug);

            for (int i = 0; i < keyValueArray.Length; i += 2)
            {
                if (KeyValue.ContainsKey(keyValueArray[i]))
                {
                    LogWriter.ToLog($"Ignoring same server key value {keyValueArray[i]} : {keyValueArray[i + 1]}");
                    continue;
                }
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Verbose, $"{keyValueArray[i]}:{keyValueArray[i + 1]} added");
                KeyValue.Add(keyValueArray[i], keyValueArray[i + 1]);
            }

            //todo add the location
            KeyValue.Add("region", "1");
            KeyValue.Add("country", "US");
        }

        public void UpdateDictionary(string key, string value)
        {
            KeyValue.Add(key, value);
        }
    }
}
