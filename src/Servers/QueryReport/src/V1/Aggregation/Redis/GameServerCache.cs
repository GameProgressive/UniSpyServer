using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.QueryReport.V1.Aggregation.Redis
{
    public record GameServerCache : UniSpy.Server.Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; set; }
        [RedisKey]
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress HostIPAddress { get; set; }
        [RedisKey]
        public int? HostPort { get; set; }
        [JsonIgnore]
        public IPEndPoint HostIPEndPoint => new IPEndPoint(HostIPAddress, (int)HostPort);
        [RedisKey]
        public string GameName { get; set; }
        public bool IsValidated { get; set; }
        /// <summary>
        /// The key values that contians all the information about this game server
        /// </summary>
        public Dictionary<string, string> KeyValues { get; set; }
        public GameServerCache() : base(RedisDbNumber.GameServerV1, TimeSpan.FromSeconds(30))
        {
        }
        public class RedisClient : RedisClient<GameServerCache>
        {
            public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection)
            {
            }
        }
    }
}