using UniSpyServer.NatNegotiation.Entity.Enumerate;
using Newtonsoft.Json;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.NatNegotiation.Entity.Structure.Redis
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
            Db = UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg;
        }
    }
}
