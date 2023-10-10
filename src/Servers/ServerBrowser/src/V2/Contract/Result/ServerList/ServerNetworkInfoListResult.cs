using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;

namespace UniSpy.Server.ServerBrowser.V2.Contract.Result.ServerList
{

    public sealed class ServerNetworkInfoListResult : ServerMainListResult
    {
        public List<GameServerCache> ServersInfo { get; private set; }
        public ServerNetworkInfoListResult()
        {
            ServersInfo = new List<GameServerCache>();
        }
    }
}
