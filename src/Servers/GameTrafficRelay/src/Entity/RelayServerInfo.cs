using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extensions;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.GameTrafficRelay.Entity
{
    public record RelayServerInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid ServerID { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        public int ClientCount { get; set; }

        public RelayServerInfo() : base(TimeSpan.FromMinutes(1))
        {
        }
    }

    public class RedisClient : UniSpy.LinqToRedis.RedisClient<RelayServerInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)DbNumber.GameTrafficRelay) { }
    }
}