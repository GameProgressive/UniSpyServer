using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public record RelayServerCache : UniSpy.Server.Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        public int ClientCount { get; set; }

        public RelayServerCache() : base(RedisDbNumber.GameTrafficRelay, TimeSpan.FromMinutes(1))
        {
        }
        public class RedisClient : UniSpy.Server.Core.Abstraction.BaseClass.RedisClient<RelayServerCache>
        {
            public RedisClient() { }
        }
    }
}