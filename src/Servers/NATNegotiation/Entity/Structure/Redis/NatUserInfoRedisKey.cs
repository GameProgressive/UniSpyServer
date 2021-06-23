using NatNegotiation.Entity.Enumerate;
using Newtonsoft.Json;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace NatNegotiation.Entity.Structure.Redis
{
    public class NatUserInfoRedisKey : UniSpyRedisKeyBase
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public NatPortType PortType { get; set; }
        public uint Cookie { get; set; }
        public NatUserInfoRedisKey()
        {
            DatabaseNumber = RedisDataBaseNumber.NatNeg;
        }
    }
}
