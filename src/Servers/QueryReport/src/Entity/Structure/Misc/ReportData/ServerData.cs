using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using System;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.Logging;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.ReportData
{
    public class ServerData
    {
        public Dictionary<string, string> KeyValue { get; protected set; }
        public GameServerStatus ServerStatus;
        public ServerData()
        {
            KeyValue = new Dictionary<string, string>();
            ServerStatus = GameServerStatus.Normal;
        }
        public static Dictionary<string, string> GetKeyValue(string serverData)
        {
            Dictionary<string, string> keyValue = new Dictionary<string, string>();

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
                    LogWriter.Verbose("Skiping empty key value");
                    continue;
                }

                if (keyValue.ContainsKey(keyValueArray[i]))
                {
                    // update exist value
                    if (keyValue[tempKey] == tempValue)
                    {
                        //LogWriter.ToLog($"Ignoring same server key value {tempKey} : {tempValue}");
                    }
                    else
                    {
                        keyValue[tempKey] = tempValue;
                        //LogWriter.ToLog($"Updated server key value {tempKey} : {tempValue}");
                    }
                }
                else
                {
                    //LogWriter.ToLog($"Added new server key value {tempKey}:{tempValue}");
                    keyValue.Add(tempKey, tempValue);
                }
            }
            return keyValue;
        }
        public void Update(string serverData)
        {
            // var keyValue = GetKeyValue(serverData);
            // foreach (var item in keyValue)
            // {
            //     if (KeyValue.ContainsKey(item.Key))
            //     {
            //         if (KeyValue[item.Key] != item.Value)
            //         {
            //             KeyValue[item.Key] = item.Value;
            //         }
            //     }
            //     else
            //     {
            //         KeyValue.Add(item.Key, item.Value);
            //     }
            // }

            LogWriter.Debug(
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
                    LogWriter.Verbose("Skiping empty key value");
                    continue;
                }

                if (KeyValue.ContainsKey(keyValueArray[i]))
                {
                    // update exist value
                    if (KeyValue[tempKey] == tempValue)
                    {
                        //LogWriter.ToLog($"Ignoring same server key value {tempKey} : {tempValue}");
                    }
                    else
                    {
                        KeyValue[tempKey] = tempValue;
                        //LogWriter.ToLog($"Updated server key value {tempKey} : {tempValue}");
                    }
                }
                else
                {
                    //LogWriter.ToLog($"Added new server key value {tempKey}:{tempValue}");
                    KeyValue.Add(tempKey, tempValue);
                }
            }
            UpdateOtherInfo();
        }
        private void UpdateOtherInfo()
        {
            ////todo add the location
            if (!KeyValue.ContainsKey("region") && !KeyValue.ContainsKey("country"))
            {
                KeyValue.Add("region", "1");
                KeyValue.Add("country", "US");
            }

            if (KeyValue.ContainsKey("statechanged"))
            {
                GameServerStatus status;
                if (Enum.TryParse(KeyValue["statechanged"], out status))
                {
                    ServerStatus = status;
                }
                else
                {
                    ServerStatus = GameServerStatus.Shutdown;
                }
            }
            else
            {
                ServerStatus = GameServerStatus.Shutdown;
            }
        }
        public void UpdateDictionary(string key, string value)
        {
            KeyValue.Add(key, value);
        }
    }
}
