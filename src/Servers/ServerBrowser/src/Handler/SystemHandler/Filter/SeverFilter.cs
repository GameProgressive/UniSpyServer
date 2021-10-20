using UniSpyServer.QueryReport.Entity.Structure.Redis;
using System.Collections.Generic;
using System.Net;

namespace UniSpyServer.ServerBrowser.Handler.CmdHandler.ServerList.GetServers.Filter
{
    public class ServerFilter
    {
        public static IEnumerable<KeyValuePair<EndPoint, GameServerInfo>> GetFilteredServer(IEnumerable<KeyValuePair<EndPoint, GameServerInfo>> rawServer, string filter)
        {
            //TODO
            //We filter server for next step
            return rawServer;
        }
    }
}
