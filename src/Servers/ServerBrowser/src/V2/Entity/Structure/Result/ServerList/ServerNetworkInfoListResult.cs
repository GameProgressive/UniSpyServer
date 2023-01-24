using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;

namespace UniSpyServer.Servers.ServerBrowser.V2.Entity.Structure.Result.ServerList
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
