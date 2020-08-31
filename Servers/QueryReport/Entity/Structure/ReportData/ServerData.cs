using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using Serilog.Events;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.ReportData
{
    public class ServerData
    {
        public Dictionary<string, string> KeyValue { get; protected set; }

        public ServerData()
        {
            KeyValue = new Dictionary<string, string>();
        }

        public void Update(string serverData)
        {
            LogWriter.ToLog(LogEventLevel.Debug,
                StringExtensions.ReplaceUnreadableCharToHex(serverData));

            string[] keyValueArray = serverData.Split("\0");

            for (int i = 0; i < keyValueArray.Length; i += 2)
            {
                if (i + 2 > keyValueArray.Length)
                {
                    break;
                }

                string tempKey = keyValueArray[i];
                string tempValue = keyValueArray[i + 1];
                if (tempKey == "")
                {
                    LogWriter.ToLog(LogEventLevel.Verbose, "Skiping empty key value");
                    continue;
                }

                if (KeyValue.ContainsKey(keyValueArray[i]))
                {
                    // update exist value
                    if (KeyValue[tempKey] == tempValue)
                    {
                        LogWriter.ToLog($"Ignoring same server key value {tempKey} : {tempValue}");
                    }
                    else
                    {
                        KeyValue[tempKey] = tempValue;
                        LogWriter.ToLog($"Updated server key value {tempKey} : {tempValue}");
                    }
                }
                else
                {
                    LogWriter.ToLog($"Added new server key value {tempKey}:{tempValue}");
                    KeyValue.Add(tempKey, tempValue);
                }
            }

            ////todo add the location
            //KeyValue.Add("region", "1");
            //KeyValue.Add("country", "US");
        }

        public void UpdateDictionary(string key, string value)
        {
            KeyValue.Add(key, value);
        }
    }
}
