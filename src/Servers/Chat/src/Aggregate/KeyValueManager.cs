using System.Collections.Concurrent;
using System.Collections.Generic;


namespace UniSpy.Server.Chat.Aggregate
{
    public class KeyValueManager
    {
        public ConcurrentDictionary<string, string> Dict { get; private set; } = new ConcurrentDictionary<string, string>();

        public KeyValueManager()
        {
        }
        
        public void Update(Dictionary<string, string> data)
        {
            // TODO check if all key is send through the request or
            // TODO only updated key send through the request
            foreach (var key in data.Keys)
            {
                //we update the key value
                Dict[key] = data[key];
            }
        }
        public void Update(KeyValuePair<string, string> data)
        {
            Dict[data.Key] = data.Value;
        }
        public static string BuildKeyValueString(Dictionary<string, string> keyValues)
        {
            string flags = "";
            foreach (var kv in keyValues)
            {
                flags += $@"\{kv.Key}\{kv.Value}";
            }
            return flags;
        }
        public string GetValueString(List<string> keys)
        {
            string values = "";
            foreach (var key in keys)
            {
                if (Dict.ContainsKey(key))
                {
                    values += @"\" + Dict[key];
                }
                else
                {
                    values += @"\";
                    // throw new ChatException($"Can not find key: {key}");
                }
            }
            return values;
        }

        public bool IsContainAllKey(List<string> keys)
        {
            foreach (var key in keys)
            {
                if (!Dict.ContainsKey(key))
                {
                    return false;
                }
            }
            return true;
        }
    }
}