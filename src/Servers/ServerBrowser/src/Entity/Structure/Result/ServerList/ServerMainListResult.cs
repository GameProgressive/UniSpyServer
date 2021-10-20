using UniSpyServer.QueryReport.Entity.Structure.Redis;
using UniSpyServer.ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.ServerBrowser.Entity.Structure.Result
{
    public class ServerMainListResult : ServerListUpdateOptionResultBase
    {
        public List<GameServerInfo> GameServerInfos { get; set; }
        public ServerMainListResult()
        {
            GameServerInfos = new List<GameServerInfo>();
        }
    }
}
