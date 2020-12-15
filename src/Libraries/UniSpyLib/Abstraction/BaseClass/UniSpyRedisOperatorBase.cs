using System;
using UniSpyLib.Extensions;
using System.Collections.Generic;

namespace UniSpyLib.Abstraction.BaseClass
{
    public abstract class UniSpyRedisOperatorBase
    {
        protected RedisDBNumber _dbNumber;
        protected TimeSpan? _expireTime;
        public UniSpyRedisOperatorBase(RedisDBNumber dBNumber)
        {
            _dbNumber = dBNumber;
            //_expireTime = TimeSpan.FromMinutes(5);
        }

        protected bool BasicSetKeyValue<T>(string key, T value)
        {
            return RedisExtensions.SetKeyValue(key, value, _dbNumber, _expireTime);
        }
        protected T BasicGetValue<T>(string key)
        {
            return RedisExtensions.GetValue<T>(key, _dbNumber);
        }
        protected Dictionary<string, T> GetAllKeyValues<T>()
        {
            return RedisExtensions.GetAllKeyValues<T>(_dbNumber);
        }

        public bool DeleteKeyValue(string key)
        {
            return RedisExtensions.DeleteKeyValue(key, _dbNumber);
        }
        public List<string> GetMatchedKeys(string subKey)
        {
            return RedisExtensions.GetMatchedKeys(subKey, _dbNumber);
        }
        public List<string> GetAllKeys()
        {
            return RedisExtensions.GetAllKeys(_dbNumber);
        }
    }
}
