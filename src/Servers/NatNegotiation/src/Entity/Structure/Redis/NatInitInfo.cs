using System;
using System.Net;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.LinqToRedis;
using UniSpyServer.Servers.NatNegotiation.Entity.Enumerate;
using UniSpyServer.UniSpyLib.MiscMethod;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Request;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis
{
    public record NatMappingInfo
    {
        public byte? Version { get; init; }
        public NatPortType? PortType { get; init; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PublicIPEndPoint { get; init; }
        /// <summary>
        /// The nat negotiation private ip and port using as p2p port
        /// nn1,nn2,nn3 is using to detect NAT type
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint PrivateIPEndPoint { get; init; }
    }
    public record NatInitInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerID { get; init; }
        [RedisKey]
        public uint? Cookie { get; init; }

        [RedisKey]
        public byte? Version { get; init; }
        [RedisKey]
        public NatClientIndex? ClientIndex { get; init; }
        /// <summary>
        /// The public ip for other client connect
        /// </summary>
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint GuessedPublicIPEndPoint { get; set; }

        public int? RetryNatNegotiationTime { get; set; }
        public byte? UseGamePort { get; init; }
        /// <summary>
        /// Is using UniSpyGameRelay to relay game traffic for clients, default value is false
        /// </summary>
        public bool IsUsingRelayServer { get; set; }

        public ConcurrentDictionary<NatPortType, NatMappingInfo> NatMappingInfos { get; }

        public NatInitInfo() : base(TimeSpan.FromMinutes(3))
        {
            NatMappingInfos = new ConcurrentDictionary<NatPortType, NatMappingInfo>();
        }
    }

    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NatInitInfo>
    {
        public RedisClient() : base(Client.RedisConnection, (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg) { }
    }
}