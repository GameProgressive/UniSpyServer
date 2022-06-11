using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.UniSpyLib.Extensions;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity.Structure.Redis
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
        public RedisClient() : base(Client.RedisConnection, (int)DbNumber.GameTrafficRelay) { }
    }
}