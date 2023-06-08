using System;
using System.Net;
using Newtonsoft.Json;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Extension.Redis;
using UniSpy.Server.Core.Misc;
using UniSpy.Server.NatNegotiation.Enumerate;

namespace UniSpy.Server.NatNegotiation.Aggregate.Redis.Fail
{
    /// <summary>
    /// The information pair using to switch strategy
    /// </summary>
    /// <value></value>
    public record NatFailInfo : UniSpy.Server.Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress PublicIPAddress1 { get; init; }
        [RedisKey]
        [JsonConverter(typeof(IPAddresConverter))]
        public IPAddress PublicIPAddress2 { get; init; }
        public NatFailInfo() : base(RedisDbNumber.NatFailInfo, TimeSpan.FromDays(1)) { }
        public NatFailInfo(NatInitInfo info1, NatInitInfo info2) : base(RedisDbNumber.NatFailInfo, TimeSpan.FromDays(1))
        {
            // we need to store in sequence to make consistancy and reduce duplications
            if (info1.ClientIndex == NatClientIndex.GameClient)
            {
                PublicIPAddress1 = info1.PublicIPEndPoint.Address;
                PublicIPAddress2 = info2.PublicIPEndPoint.Address;
            }
            else
            {
                PublicIPAddress1 = info2.PublicIPEndPoint.Address;
                PublicIPAddress2 = info1.PublicIPEndPoint.Address;
            }
        }
    }

    public class RedisClient : UniSpy.Server.Core.Abstraction.BaseClass.RedisClient<NatFailInfo>
    {
        public RedisClient() { }
    }
}