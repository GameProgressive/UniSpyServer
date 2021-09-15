using NatNegotiation.Entity.Enumerate;
using Newtonsoft.Json;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;
using UniSpyLib.MiscMethod;

namespace NatNegotiation.Entity.Structure.Redis
{
    public class UserInfoRedisKey : UniSpyRedisKey
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public NatPortType PortType { get; set; }
        public uint Cookie { get; set; }
        public UserInfoRedisKey()
        {
            DatabaseNumber = RedisDataBaseNumber.NatNeg;
        }
    }
}
