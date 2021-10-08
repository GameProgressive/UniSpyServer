using Newtonsoft.Json;
using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Redis
{
    public class GameServerInfoRedisKey : UniSpyRedisKey
    {
        [JsonProperty(Order = -2, NullValueHandling = NullValueHandling.Ignore)]
        public Guid? ServerID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IPEndPoint RemoteIPEndPoint { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public uint? InstantKey { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string GameName { get; set; }
        public GameServerInfoRedisKey()
        {
            Db = UniSpyLib.Extensions.DbNumber.GameServer;
        }
    }
}
