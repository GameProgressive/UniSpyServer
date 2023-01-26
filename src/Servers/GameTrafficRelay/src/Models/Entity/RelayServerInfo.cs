using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.GameTrafficRelay.Application;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity
{
    public record RelayServerInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid ServerID { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        public int ClientCount { get; init; }

        public RelayServerInfo() : base(TimeSpan.FromMinutes(1))
        {
        }
    }

    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<RelayServerInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)DbNumber.GameTrafficRelay) { }
    }
}