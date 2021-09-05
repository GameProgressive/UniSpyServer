using QueryReport.Entity.Structure.Redis;
using ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;

namespace ServerBrowser.Entity.Structure.Result
{
    internal class ServerMainListResult : ServerListUpdateOptionResultBase
    {
        public List<GameServerInfo> GameServerInfos { get; set; }
        public ServerMainListResult()
        {
            GameServerInfos = new List<GameServerInfo>();
        }
    }
}
