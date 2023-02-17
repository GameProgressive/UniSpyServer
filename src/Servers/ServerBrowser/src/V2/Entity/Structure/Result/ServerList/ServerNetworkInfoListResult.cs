using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Redis.GameServer;

namespace UniSpy.Server.ServerBrowser.V2.Entity.Structure.Result.ServerList
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
