using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;

namespace UniSpyServer.Servers.ServerBrowser.Entity.Structure.Result.ServerList
{

    public sealed class ServerNetworkInfoListResult : ServerMainListResult
    {
        public List<GameServerInfo> ServersInfo { get; private set; }
        public ServerNetworkInfoListResult()
        {
            ServersInfo = new List<GameServerInfo>();
        }
    }
}
