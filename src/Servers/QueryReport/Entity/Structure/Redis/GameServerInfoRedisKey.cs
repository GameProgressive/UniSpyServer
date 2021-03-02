using System;
using System.Net;
using Newtonsoft.Json;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Redis
{
    public class GameServerInfoRedisKey : UniSpyRedisKeyBase
    {
        [JsonProperty(Order = -2)]
        public Guid ServerID { get; set; }
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public string GameName { get; set; }
        public GameServerInfoRedisKey()
        {
        }
    }
}
