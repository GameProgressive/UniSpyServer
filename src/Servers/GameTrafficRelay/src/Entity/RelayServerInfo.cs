using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.GameTrafficRelay.Entity
{
    public record RelayServerInfo : UniSpy.Server.Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        public int ClientCount { get; set; }

        public RelayServerInfo() : base(RedisDbNumber.GameTrafficRelay, TimeSpan.FromMinutes(1))
        {
        }
    }

    public class RedisClient : UniSpy.Server.Core.Abstraction.BaseClass.RedisClient<RelayServerInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection) { }
    }
}