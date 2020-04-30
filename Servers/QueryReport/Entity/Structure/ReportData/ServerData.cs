using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
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
            LogWriter.ToLog(LogEventLevel.Debug,
                StringExtensions.ReplaceUnreadableCharToHex(serverData));

            KeyValue.Clear();
            string[] keyValueArray = serverData.Split("\0");

            for (int i = 0; i < keyValueArray.Length; i += 2)
            {
                if (KeyValue.ContainsKey(keyValueArray[i]))
                {
                    LogWriter.ToLog($"Ignoring same server key value {keyValueArray[i]} : {keyValueArray[i + 1]}");
                    continue;
                }

                if (keyValueArray[i] == "")
                {
                    LogWriter.ToLog(LogEventLevel.Verbose, "Skiping empty key value");
                    continue;
                }
                LogWriter.ToLog(LogEventLevel.Verbose, $"{keyValueArray[i]}:{keyValueArray[i + 1]}");
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
