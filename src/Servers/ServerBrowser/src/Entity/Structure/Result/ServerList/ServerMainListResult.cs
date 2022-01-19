using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis;
using UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result
{
    public class ServerMainListResult : ServerListUpdateOptionResultBase
    {
        public List<GameServerInfo2> GameServerInfos { get; set; }
        public ServerMainListResult()
        {
            GameServerInfos = new List<GameServerInfo2>();
        }
    }
}
