﻿using Newtonsoft.Json;
using System;
using System.Net;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis
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
            Db = UniSpyServer.UniSpyLib.Extensions.DbNumber.GameServer;
        }
    }
}
