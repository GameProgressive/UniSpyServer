using Newtonsoft.Json;
using System;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis
{
    public abstract class UniSpyRedisKey
    {
        [JsonIgnore]
        public string SearchKey => BuildSearchKey();
        [JsonIgnore]
        public string FullKey => BuildFullKey();
        [JsonIgnore]
        public DbNumber? Db { get; protected set; }
        [JsonIgnore]
        public TimeSpan? ExpireTime { get; protected set; }
        public UniSpyRedisKey()
        {
        }
        public UniSpyRedisKey(DbNumber? db, TimeSpan? expireTime)
        {
            Db = db;
            ExpireTime = expireTime;
        }
        public UniSpyRedisKey(DbNumber? databaseNumber)
        {
            Db = databaseNumber;
        }

        private string BuildFullKey()
        {
            foreach (var property in GetType().GetFields())
            {
                if (property.GetValue(this) == null)
                {
                    throw new ArgumentNullException();
                }
            }
            var redisKey = JsonConvert.SerializeObject(this);
            return redisKey;
        }

        private string BuildSearchKey()
        {
            // string searchKey = "";
            // foreach (var property in this.GetType().GetProperties())
            // {
            //     searchKey = $"{searchKey}:{property.Name}={property.GetValue(this)}";
            // }
            var redisKey = JsonConvert.SerializeObject(this);
            redisKey = redisKey.Replace("{", "*").Replace("}", "*")
            .Replace(",", "*").Replace("\n", "");

            return redisKey;
        }
    }
}
