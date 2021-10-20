using System;
using System.Net;
using UniSpyServer.NatNegotiation.Application;
using UniSpyServer.NatNegotiation.Entity.Enumerate;
using UniSpyServer.NatNegotiation.Entity.Structure.Request;
using Newtonsoft.Json;
using StackExchange.Redis;
using UniSpyServer.UniSpyLib.MiscMethod;
using UniSpyServer.LinqToRedis;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Redis
{
    public record NewUserInfo : RedisKeyValueObject
    {
        [RedisKey]
        public Guid ServerID { get; set; }
        [RedisKey]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        [RedisKey]
        public NatPortType PortType { get; set; }
        [RedisKey]
        public uint Cookie { get; set; }
        public InitRequest RequestInfo { get; set; }
        public DateTime LastPacketRecieveTime;
        public int RetryNATNegotiationTime;
    }
    public class RedisClient : UniSpyServer.LinqToRedis.RedisClient<NewUserInfo>
    {
        public RedisClient() : base(ServerFactory.Redis, (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg)
        {
        }
    }
}