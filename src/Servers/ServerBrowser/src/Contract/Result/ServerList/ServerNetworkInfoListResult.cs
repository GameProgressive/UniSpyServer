using System.Collections.Generic;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;

namespace UniSpy.Server.ServerBrowser.Contract.Result.ServerList
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
