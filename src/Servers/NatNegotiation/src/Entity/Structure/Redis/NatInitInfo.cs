using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public record NatInitInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        public NatPortType? PortType { get; init; }
        [RedisKey]
        public uint? Cookie { get; init; }
        [RedisKey]
        public NatClientIndex? ClientIndex { get; init; }
        [RedisKey]
        public byte? Version { get; init; }

        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint GPPrivateIPEndPoint { get; init; }
        public int? RetryNatNegotiationTime { get; set; }
        public byte? UseGamePort { get; init; }
        public int RetryCount { get; set; }

        public NatInitInfo() : base(TimeSpan.FromMinutes(3))
        {
        }
    }
    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NatInitInfo>
    {
        public RedisClient() :
        base(Client.RedisConnection,
            (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg)
        { }
    }
}