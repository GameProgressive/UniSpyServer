using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result
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
