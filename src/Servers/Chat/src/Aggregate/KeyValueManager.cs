using System.Collections.Concurrent;
using System.Collections.Generic;
using UniSpy.Server.Chat.Exception;

namespace UniSpy.Server.Chat.Aggregate
{
    public class KeyValueManager
    {
        public ConcurrentDictionary<string, string> Dict { get; private set; }
        public KeyValueManager()
        {
            Dict = new ConcurrentDictionary<string, string>();
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
                    // todo check whether we need add empty value "\\"
                    throw new ChatException($"Can not find key: {key}");
                }
            }
            return values;
        }
    }
}