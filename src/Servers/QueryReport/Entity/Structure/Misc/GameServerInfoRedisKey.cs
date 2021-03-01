using System;
using System.Net;
using UniSpyLib.Abstraction.BaseClass.Redis;

namespace QueryReport.Entity.Structure.Misc
{
    public class GameServerInfoRedisKey : UniSpyRedisKeyBase
    {
        public IPEndPoint RemoteIPEndPoint { get; set; }
        public string GameName { get; set; }
        public GameServerInfoRedisKey()
        {
        }
    }
}
