using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Extensions;

namespace UniSpyLib.Abstraction.BaseClass
{
    /// <summary>
    /// This class uses generate type for easily convert Redis objects to your ideal objects.
    /// You must construct the search key or full key on your inherited class.
    /// </summary>
    /// <typeparam name="T">The serialization type</typeparam>
    public abstract class UniSpyRedisOperatorBase<T>
    {
        protected RedisDBNumber? _dbNumber;
        protected TimeSpan? _timeSpan;

        public UniSpyRedisOperatorBase(RedisDBNumber dbNumber)
        {
            _dbNumber = dbNumber;
            _timeSpan = null;
        }

        public virtual bool SetKeyValue(string key, T value)
        {
            var jsonStr = JsonConvert.SerializeObject(value);
            return RedisExtensions.SetKeyValue(key, jsonStr, _dbNumber);
        }

        public virtual T GetSpecificValue(string fullKey)
        {
            var value = RedisExtensions.GetSpecificValue(fullKey, _dbNumber);
            if (value == null)
            {
                return default(T);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
        }

        public bool DeleteKeyValue(string fullKey)
        {
            return RedisExtensions.DeleteKeyValue(fullKey, _dbNumber);
        }
        public List<string> GetMatchedKeys(string searchKey)
        {
            return RedisExtensions.GetMatchedKeys(searchKey, _dbNumber);
        }

        public List<string> GetAllKeys()
        {
            return RedisExtensions.GetAllKeys(_dbNumber);
        }

        public virtual Dictionary<string, T> GetMatchedKeyValues(string searchKey)
        {
            var kvs = RedisExtensions.GetMatchedKeyValues(searchKey, _dbNumber);
            var newDict = kvs.ToDictionary(k => k.Key, k => JsonConvert.DeserializeObject<T>(k.Value));
            return newDict;
        }

        public virtual Dictionary<string, T> GetAllKeyValues()
        {
            var kv = RedisExtensions.GetAllKeyValues(_dbNumber);
            var newDict = kv.ToDictionary(k => k.Key, k => JsonConvert.DeserializeObject<T>(k.Value));
            return newDict;
        }

        protected string BuildFullKey(params object[] obj)
        {
            string key = "";
            for (int i = 0; i < obj.Length; i++)
            {
                if (i == 0)
                {
                    key += obj[i].ToString();
                }
                else
                {
                    key += " " + obj[i].ToString();
                }
            }
            return key;
        }

        protected string BuildSearchKey(params object[] obj)
        {
            string key = "*";
            for (int i = 0; i < obj.Length; i++)
            {
                key += obj[i].ToString() + "*";
            }
            return key;
        }
    }
}
